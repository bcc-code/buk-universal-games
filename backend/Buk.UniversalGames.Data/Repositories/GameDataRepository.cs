﻿using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Library.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Buk.UniversalGames.Data.Repositories
{
    public class GameDataRepository : IGameRepository
    {
        private readonly DataContext _db;

        public GameDataRepository(DataContext db)
        {
            _db = db;
        }

        public Task<List<Game>> GetGames()
        {
            return _db.Games.ToListAsync();
        }

        public async Task<List<MatchListItem>> GetMatches(Team team)
        {
            return await (
                from match in _db.Matches
                where match.Team1Id == team.TeamId || match.Team2Id == team.TeamId
                orderby match.Start
                select new MatchListItem
                {
                    MatchId = match.MatchId,
                    GameId = match.GameId,
                    AddOn = match.AddOn,
                    Team1Id = match.Team1Id,
                    Team1 = match.Team1.Name,
                    Team2Id = match.Team2Id,
                    Team2 = match.Team2.Name,
                    WinnerId = match.WinnerId.GetValueOrDefault(),
                    Winner = match.WinnerId.HasValue ? (match.WinnerId.Value == match.Team1Id ? match.Team1.Name : match.Team2.Name) : "",
                    Start = match.Start.ToLocalTime().ToString("HH:mm"),
                }).ToListAsync();
        }
        public async Task<List<MatchListItem>> GetMatches(int leagueId, int? gameId = null)
        {
            return await (
                from match in _db.Matches
                join pointsreg1 in _db.Points on new { MatchId = (int?)match.MatchId, TeamId = match.Team1Id } equals new { pointsreg1.MatchId, pointsreg1.TeamId } into joinedPoints1
                from pointsreg1 in joinedPoints1.DefaultIfEmpty()
                join pointsreg2 in _db.Points on new { MatchId = (int?)match.MatchId, TeamId = match.Team2Id } equals new { pointsreg2.MatchId, pointsreg2.TeamId } into joinedPoints2
                from pointsreg2 in joinedPoints2.DefaultIfEmpty()
                where match.LeagueId == leagueId && (!gameId.HasValue || match.GameId == gameId.Value)
                    orderby match.Start, match.GameId, match.Team1.Name
                    select new MatchListItem
                    {
                        MatchId = match.MatchId,
                        GameId = match.GameId,
                        AddOn = match.AddOn,
                        Team1Id = match.Team1Id,
                        Team1 = match.Team1.Name,
                        Team2Id = match.Team2Id,
                        Team2 = match.Team2.Name,
                        WinnerId = match.WinnerId.GetValueOrDefault(),
                        Winner = match.WinnerId.HasValue ? (match.WinnerId!.Value == match.Team1Id ? match.Team1.Name : match.Team2.Name) : "",
                        Team1Result = pointsreg1.Points,
                        Team2Result = pointsreg2.Points,
                        Start = match.Start.ToLocalTime().ToString("HH:mm"),
                    }).ToListAsync();
        }

        public async Task<MatchWinnerResult> SetMatchWinner(Game game, int matchId, Team winningTeam)
        {
            var match = await _db.Matches
                .AsTracking()
                .FirstOrDefaultAsync(s => s.MatchId == matchId)
                ?? throw new BadRequestException(Strings.UnknownMatchId);
            var existingPointsRegistrations = await _db.Points.Where(s => s.MatchId == matchId).ToListAsync();

            match.WinnerId = winningTeam.TeamId;

            PointsRegistration? winningPointsRegistration = null;
            PointsRegistration? losingPointsRegistration = null;

            if (existingPointsRegistrations.Count > 0)
            {
                foreach (var pointsRegistration in existingPointsRegistrations)
                {
                    if (pointsRegistration.TeamId == winningTeam.TeamId)
                    {
                        pointsRegistration.Points = game.WinnerPoints;
                        winningPointsRegistration = pointsRegistration;
                    }
                    else
                    {
                        pointsRegistration.Points = game.LooserPoints;
                        losingPointsRegistration = pointsRegistration;
                    }
                }
            }
            else
            {
                winningPointsRegistration = new PointsRegistration
                {
                    TeamId = winningTeam.TeamId,
                    Points = game.WinnerPoints,
                    MatchId = matchId,
                    GameId = game.GameId,
                    Added = DateTime.Now,
                };
                
                var losingTeamId = match.Team1Id == winningTeam.TeamId ? match.Team2Id : match.Team1Id;

                losingPointsRegistration = new PointsRegistration
                {
                    TeamId = losingTeamId,
                    Points = game.LooserPoints,
                    MatchId = matchId,
                    GameId = game.GameId,
                    Added = DateTime.Now
                };

                await _db.Points.AddRangeAsync(winningPointsRegistration, losingPointsRegistration);
            }

            await _db.SaveChangesAsync();

            return new MatchWinnerResult(match, winningPointsRegistration, losingPointsRegistration);
        }

        public async Task<MatchListItem> StoreMatchResult(Match match, int teamId, int measuredResult)
        {
            var existingRegistrations = await _db.Points.Where(s => s.MatchId == match.MatchId).AsTracking().ToListAsync();
            var currentTeamResult = existingRegistrations.FirstOrDefault(x => x.TeamId == teamId);
            var otherTeamResult = existingRegistrations.FirstOrDefault(x => x.TeamId != teamId);

            if (currentTeamResult != null)
            {
                currentTeamResult.Points = measuredResult;
            }
            else
            {
                var addedEntry = _db.Points.Add(new PointsRegistration { MatchId = match.MatchId, TeamId = teamId, GameId = match.GameId, Points = measuredResult });
                currentTeamResult = addedEntry.Entity;
            }

            if (otherTeamResult is not null)
            {
                if (measuredResult == otherTeamResult.Points)
                {
                    match.WinnerId = null; //no winner
                }
                else
                {
                    //set winner and save
                    if (match.GameId == 2 || match.GameId == 3) //tickettwist + monkeybars: higher is better
                    {
                        match.WinnerId = measuredResult > otherTeamResult.Points ? teamId : otherTeamResult.TeamId;
                    }
                    else
                    {
                        match.WinnerId = measuredResult < otherTeamResult.Points ? teamId : otherTeamResult.TeamId;
                    }
                }
            }
            await _db.SaveChangesAsync();

            return new MatchListItem
            {
                MatchId = match.MatchId,
                GameId = match.GameId,
                AddOn = match.AddOn,
                Team1Id = match.Team1Id,
                Team2Id = match.Team2Id,
                WinnerId = match.WinnerId.GetValueOrDefault(),
                Team1Result = teamId == match.Team1Id ? measuredResult : otherTeamResult?.Points,
                Team2Result = teamId == match.Team2Id ? measuredResult : otherTeamResult?.Points,
                Start = match.Start.ToLocalTime().ToString("HH:mm"),
            };
        }
    }
}
