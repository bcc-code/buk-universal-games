using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Interfaces
{
    public interface ILeagueService
    {
        IEnumerable<League> GetLeagues();

        IEnumerable<Team> GetTeams(int leagueId);
    }
}
