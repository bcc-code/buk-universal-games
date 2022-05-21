using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Repositories
{
    public class LeagueDataRepository : ILeagueRepository
    {
        private readonly DataContext _db;

        public LeagueDataRepository(DataContext db)
        {
            _db = db;
        }

        public IEnumerable<League> GetLeagues()
        {
            return _db.Leagues.ToList();
        }

        public IEnumerable<Team> GetTeams(int leagueId)
        {
            return _db.Teams.Where(s => s.LeagueId == leagueId).ToList();
        }
    }
}
