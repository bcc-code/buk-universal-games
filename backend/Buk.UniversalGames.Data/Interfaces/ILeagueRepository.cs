using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface ILeagueRepository
    {
        IEnumerable<League> GetLeagues();

        IEnumerable<Team> GetTeams(int leagueId);
    }
}
