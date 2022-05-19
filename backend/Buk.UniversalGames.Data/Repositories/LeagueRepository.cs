using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        public IEnumerable<League> GetLeagues()
        {
            using var context = new DataContext();
            return context.Leagues.ToList();
        }

        public IEnumerable<Team> GetTeams(int leagueId)
        {
            using var context = new DataContext();
            return context.Teams.Where(s => s.LeagueId == leagueId).ToList();
        }
    }
}
