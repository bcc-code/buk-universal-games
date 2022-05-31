using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

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

        public List<MatchListItem> GetMatches(Team team)
        {
            return (from match in _db.Matches
                where match.Team1Id == team.TeamId || match.Team2Id == team.TeamId
                join team1 in _db.Teams on match.Team1Id equals team1.TeamId
                join team2 in _db.Teams on match.Team2Id equals team2.TeamId
                orderby match.Start
                select new MatchListItem
                {
                    MatchId = match.MatchId,
                    Team1Id = match.Team1Id,
                    Team1 = team1.Name,
                    Team2Id = match.Team2Id,
                    Team2 = team2.Name,
                    WinnerId = match.WinnerId.GetValueOrDefault(),
                    Winner = match.WinnerId.HasValue ? (match.WinnerId.Value == match.Team1Id ? team1.Name : team2.Name) : "",
                    Start = match.Start.ToShortTimeString(),
                }).ToList();
        }
    }
}
