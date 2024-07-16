using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Library.Cultures;
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

        public async Task<Match> GetMatch(int matchId)
        {
            return await (
                from match in _db.Matches
                where match.MatchId == matchId
                select new Match
                {
                    MatchId = match.MatchId,
                    GameId = match.GameId,
                    AddOn = match.AddOn,
                    Team1Id = match.Team1Id,
                    Team1 = match.Team1,
                    Team2Id = match.Team2Id,
                    Team2 = match.Team2,
                    WinnerId = match.WinnerId.GetValueOrDefault(),
                    Start = match.Start.ToLocalTime(),
                    Game = match.Game,
                    League = match.League,
                    LeagueId = match.LeagueId,
                    Position = match.Position
                }
            ).FirstAsync();
        }

        public async Task<List<MatchListItem>> GetMatches(Team team)
        {
            return await (
                from match in _db.Matches
                join pointsreg1 in _db.Points on new { MatchId = (int?)match.MatchId, TeamId = match.Team1Id } equals new { pointsreg1.MatchId, pointsreg1.TeamId } into joinedPoints1
                from pointsreg1 in joinedPoints1.DefaultIfEmpty()
                join pointsreg2 in _db.Points on new { MatchId = (int?)match.MatchId, TeamId = match.Team2Id } equals new { pointsreg2.MatchId, pointsreg2.TeamId } into joinedPoints2
                from pointsreg2 in joinedPoints2.DefaultIfEmpty()
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
                    Start = match.Start.ToLocalTime().ToString("HH:mm"),
                    Team1Result = pointsreg1.Points,
                    Team2Result = pointsreg2.Points,
                    Position = match.Position,
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
                orderby match.Start, match.GameId, match.Position
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
                    Team1Result = pointsreg1.Points,
                    Team2Result = pointsreg2.Points,
                    Start = match.Start.ToLocalTime().ToString("HH:mm"),
                    Position = match.Position,
                }).ToListAsync();
        }

        public async Task<MatchListItem> StoreMatchResult(Match match, int teamId, int measuredResult)
        {
            if (match == null) throw new ArgumentNullException(nameof(match), "Match cannot be null");

            var existingRegistrations = await _db.Points.Where(s => s.MatchId == match.MatchId).AsTracking().ToListAsync();
            var currentTeamResult = existingRegistrations.FirstOrDefault(x => x.TeamId == teamId);
            var otherTeamResult = existingRegistrations.FirstOrDefault(x => x.TeamId != teamId);

            if (currentTeamResult != null)
            {
                currentTeamResult.Points = measuredResult;
            }
            else
            {
                if (match.Team1 == null) throw new InvalidOperationException("match.Team1 is null");
                if (match.GameId == 0) throw new InvalidOperationException("match.GameId is not set");

                var addedEntry = _db.Points.Add(new PointsRegistration
                {
                    MatchId = match.MatchId,
                    TeamId = teamId,
                    Team = match.Team1,
                    GameId = match.GameId,
                    Points = measuredResult
                });

                currentTeamResult = addedEntry.Entity;
            }

            await _db.SaveChangesAsync();

            if (match.Team1 == null) throw new InvalidOperationException("match.Team1 is null after save");
            if (match.Team2 == null) throw new InvalidOperationException("match.Team2 is null after save");

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
                Position = match.Position,
                Team1 = match.Team1?.Name ?? "Unknown",  // Safeguard for null
                Team2 = match.Team2?.Name ?? "Unknown",  // Safeguard for null
            };
        }

        public async Task<MatchListItem> StoreMatchResults(Match match, int team1Result, int team2Result)
        {
            if (match == null) throw new ArgumentNullException(nameof(match), "Match cannot be null");

            var existingRegistrations = await _db.Points.Where(s => s.MatchId == match.MatchId).AsTracking().ToListAsync();
            if (existingRegistrations.Count != 0 && existingRegistrations.Count != 2)
            {
                throw new InvalidOperationException("There must be either 0 or 2 existing points entries for the match.");
            }

            PointsRegistration team1Points, team2Points;

            if (existingRegistrations.Count == 0)
            {
                team1Points = _db.Points.Add(new PointsRegistration
                {
                    MatchId = match.MatchId,
                    TeamId = match.Team1Id,
                    Team = match.Team1,
                    GameId = match.GameId,
                    Points = team1Result,
                    Added = DateTime.UtcNow
                }).Entity;

                team2Points = _db.Points.Add(new PointsRegistration
                {
                    MatchId = match.MatchId,
                    TeamId = match.Team2Id,
                    Team = match.Team2,
                    GameId = match.GameId,
                    Points = team2Result,
                    Added = DateTime.UtcNow
                }).Entity;
            }
            else
            {
                team1Points = existingRegistrations.First(x => x.TeamId == match.Team1Id);
                team1Points.Points = team1Result;
                team1Points.Added = DateTime.UtcNow;

                team2Points = existingRegistrations.First(x => x.TeamId == match.Team2Id);
                team2Points.Points = team2Result;
                team2Points.Added = DateTime.UtcNow;
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
                Team1Result = team1Result,
                Team2Result = team2Result,
                Start = match.Start.ToLocalTime().ToString("HH:mm"),
                Position = match.Position,
                Team1 = match.Team1?.Name ?? "Unknown",
                Team2 = match.Team2?.Name ?? "Unknown",
            };
        }
    }
}
