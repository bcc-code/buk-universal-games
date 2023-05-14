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

        public async Task<List<MatchListItem>> GetMatches(Team team)
        {
            return await (
                from match in _db.Matches
                join team1 in _db.Teams on match.Team1Id equals team1.TeamId
                join team2 in _db.Teams on match.Team2Id equals team2.TeamId
                where match.Team1Id == team.TeamId || match.Team2Id == team.TeamId
                orderby match.Start
                select new MatchListItem
                {
                    MatchId = match.MatchId,
                    GameId = match.GameId,
                    AddOn = match.AddOn,
                    Team1Id = match.Team1Id,
                    Team1 = team1.Name,
                    Team2Id = match.Team2Id,
                    Team2 = team2.Name,
                    WinnerId = match.WinnerId.GetValueOrDefault(),
                    Winner = match.WinnerId.HasValue ? (match.WinnerId.Value == match.Team1Id ? team1.Name : team2.Name) : "",
                    Start = match.Start.ToShortTimeString(),
                }).ToListAsync();
        }
        public async Task<List<MatchListItem>> GetMatches(int leagueId, int? gameId = null)
        {
            return await (
                from match in _db.Matches
                    join team1 in _db.Teams on match.Team1Id equals team1.TeamId
                    join team2 in _db.Teams on match.Team2Id equals team2.TeamId
                    join game in _db.Games on match.GameId equals game.GameId
                    where (team1.LeagueId == leagueId && (!gameId.HasValue || match.GameId == gameId.Value))
                    orderby match.Start, match.GameId, team1.Name
                    select new MatchListItem
                    {
                        MatchId = match.MatchId,
                        GameId = match.GameId,
                        AddOn = match.AddOn,
                        Team1Id = match.Team1Id,
                        Team1 = team1.Name,
                        Team2Id = match.Team2Id,
                        Team2 = team2.Name,
                        WinnerId = match.WinnerId.GetValueOrDefault(),
                        Winner = match.WinnerId.HasValue ? (match.WinnerId.Value == match.Team1Id ? team1.Name : team2.Name) : "",
                        Start = match.Start.ToShortTimeString(),
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

        public async Task<TeamMatchResult> StoreMatchResult(int matchId, int teamId, int measuredResult)
        {
            var match = await _db.Matches.FindAsync(matchId) ?? throw new ArgumentException("Match does not exist", nameof(matchId));
            var existingRegistrations = await _db.Points.Where(s => s.MatchId == matchId).AsTracking().ToListAsync();
            var currentTeamResult = existingRegistrations.FirstOrDefault(x => x.TeamId == teamId);
            var otherTeamResult = existingRegistrations.FirstOrDefault(x => x.TeamId != teamId);

            if (currentTeamResult != null)
            {
                currentTeamResult.Points = measuredResult;
            }
            else
            {
                await _db.Points.AddAsync(new PointsRegistration { MatchId = matchId, TeamId = teamId, GameId = match.GameId, Points = measuredResult });
            }
            await _db.SaveChangesAsync();

            return new TeamMatchResult (
                matchId, 
                teamId == match.Team1Id ? measuredResult : otherTeamResult?.Points, 
                teamId == match.Team2Id ? measuredResult : otherTeamResult?.Points);
        }
    }
}
