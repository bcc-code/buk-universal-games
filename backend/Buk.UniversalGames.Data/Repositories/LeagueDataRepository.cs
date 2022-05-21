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

        public League? GetLeague(int leagueId)
        {
            return _db.Leagues.FirstOrDefault(s => s.LeagueId == leagueId);
        }

        public List<League> GetLeagues()
        {
            return _db.Leagues.ToList();
        }

        public Team? GetTeamByCode(string code)
        {
            return _db.Teams.FirstOrDefault(s => s.Code == code);
        }

        public List<Team> GetTeams(int leagueId)
        {
            return _db.Teams.Where(s => s.LeagueId == leagueId).ToList();
        }
    }
}
