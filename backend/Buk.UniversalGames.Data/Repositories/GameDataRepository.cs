using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Exceptions;

namespace Buk.UniversalGames.Data.Repositories
{
    public class GameDataRepository : IGameRepository
    {
        private readonly DataContext _db;

        public GameDataRepository(DataContext db)
        {
            _db = db;
        }

        public List<Game> GetGames()
        {
            return _db.Games.ToList();
        }

        public List<MatchListItem> GetMatches(int teamId)
        {
            return (from match in _db.Matches
                where match.Team1Id == teamId || match.Team2Id == teamId
                join team1 in _db.Teams on match.Team1Id equals team1.TeamId
                join team2 in _db.Teams on match.Team2Id equals team2.TeamId
                orderby match.Start
                select new MatchListItem
                {
                    MatchId = match.MatchId,
                    GameId = match.GameId,
                    Team1Id = match.Team1Id,
                    Team1 = team1.Name,
                    Team2Id = match.Team2Id,
                    Team2 = team2.Name,
                    WinnerId = match.WinnerId.GetValueOrDefault(),
                    Winner = match.WinnerId.HasValue ? (match.WinnerId.Value == match.Team1Id ? team1.Name : team2.Name) : "",
                    Start = match.Start.ToShortTimeString(),
                }).ToList();
        }
        public List<MatchListItem> GetGameMatches(int leagueId, int? gameId = null)
        {
            return (from match in _db.Matches
                    where !gameId.HasValue || match.GameId == gameId.Value
                    join team1 in _db.Teams on match.Team1Id equals team1.TeamId
                    where team1.LeagueId == leagueId
                    join team2 in _db.Teams on match.Team2Id equals team2.TeamId
                    where team2.LeagueId == leagueId
                    join game in _db.Games on match.GameId equals game.GameId
                    where game.GameId == match.GameId
                    orderby match.Start
                    select new MatchListItem
                    {
                        MatchId = match.MatchId,
                        GameId = match.GameId,
                        Team1Id = match.Team1Id,
                        Team1 = team1.Name,
                        Team2Id = match.Team2Id,
                        Team2 = team2.Name,
                        WinnerId = match.WinnerId.GetValueOrDefault(),
                        Winner = match.WinnerId.HasValue ? (match.WinnerId.Value == match.Team1Id ? team1.Name : team2.Name) : "",
                        Start = match.Start.ToShortTimeString(),
                    }).ToList();
        }

        public MatchWinnerResult SetMatchWinner(Game game, int matchId, int winnerTeamId)
        {
            var match = _db.Matches.FirstOrDefault(s => s.MatchId == matchId);

            if(match == null)
                throw new BadRequestException(Strings.UnknownMatchId);

            var points = _db.Points.Where(s => s.MatchId == matchId).ToList();

            if (match.WinnerId.GetValueOrDefault() != winnerTeamId)
                match.WinnerId = winnerTeamId;

            Point? winnerPoint = null;
            Point? looserPoint = null;

            if (points.Count > 0)
            {
                foreach (var point in points)
                {
                    if (point.TeamId == winnerTeamId)
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
                    TeamId = winnerTeamId,
                    Points = game.WinnerPoints,
                    MatchId = matchId,
                    GameId = game.GameId,
                    Added = DateTime.Now,
                };
                _db.Points.Add(winnerPoint);

                var looserTeamId = match.Team1Id == winnerTeamId ? match.Team2Id : match.Team1Id;

                looserPoint = new Point
                {
                    TeamId = looserTeamId,
                    Points = game.LooserPoints,
                    MatchId = matchId,
                    GameId = game.GameId,
                    Added = DateTime.Now
                };
                _db.Points.Add(looserPoint);
            }

            _db.SaveChanges();

            return new MatchWinnerResult
            {
                LooserPoint = looserPoint,
                WinnerPoint = winnerPoint,
                Match = match
            };
        }
    }
}
