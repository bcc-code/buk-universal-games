using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
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

        public async Task<List<Game>> GetGames()
        {
            return await _db.Games.ToListAsync();
        }

        public async Task<List<MatchListItem>> GetMatches(Team team)
        {
            return await (from match in _db.Matches
                where match.Team1Id == team.TeamId || match.Team2Id == team.TeamId
                    join team1 in _db.Teams on match.Team1Id equals team1.TeamId
                join team2 in _db.Teams on match.Team2Id equals team2.TeamId
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
        public async Task<List<MatchListItem>> GetGameMatches(int leagueId, int? gameId = null)
        {
            return await (from match in _db.Matches
                    where !gameId.HasValue || match.GameId == gameId.Value
                    join team1 in _db.Teams on match.Team1Id equals team1.TeamId
                    where team1.LeagueId == leagueId
                    join team2 in _db.Teams on match.Team2Id equals team2.TeamId
                    where team2.LeagueId == leagueId
                    join game in _db.Games on match.GameId equals game.GameId
                    where game.GameId == match.GameId
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

        public async Task<MatchWinnerResult> SetMatchWinner(Game game, int matchId, Team team)
        {
            var match = await _db.Matches.FirstOrDefaultAsync(s => s.MatchId == matchId);

            if(match == null)
                throw new BadRequestException(Strings.UnknownMatchId);

            var points = await _db.Points.Where(s => s.MatchId == matchId).ToListAsync();

            if (match.WinnerId.GetValueOrDefault() != team.TeamId)
                match.WinnerId = team.TeamId;

            Point? winnerPoint = null;
            Point? looserPoint = null;

            if (points.Count > 0)
            {
                foreach (var point in points)
                {
                    if (point.TeamId == team.TeamId)
                    {
                        point.Points = game.WinnerPoints;
                        winnerPoint = point;
                    }
                    else
                    {
                        point.Points = game.LooserPoints;
                        looserPoint = point;
                    }
                }
            }
            else
            {
                winnerPoint = new Point
                {
                    TeamId = team.TeamId,
                    Points = game.WinnerPoints,
                    MatchId = matchId,
                    GameId = game.GameId,
                    Added = DateTime.Now,
                };
                await _db.Points.AddAsync(winnerPoint);

                var looserTeamId = match.Team1Id == team.TeamId ? match.Team2Id : match.Team1Id;

                looserPoint = new Point
                {
                    TeamId = looserTeamId,
                    Points = game.LooserPoints,
                    MatchId = matchId,
                    GameId = game.GameId,
                    Added = DateTime.Now
                };
                await _db.Points.AddAsync(looserPoint);
            }

            await _db.SaveChangesAsync();

            return new MatchWinnerResult
            {
                LooserPoint = looserPoint,
                WinnerPoint = winnerPoint,
                Match = match
            };
        }
    }
}
