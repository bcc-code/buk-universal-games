using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Models;

namespace Buk.UniversalGames.Interfaces
{
    public interface IStatusService
    {
        Task<TeamStatusReport> GetTeamStatus(Team team);

        Task<LeagueStatusReport> GetLeagueStatus(int leagueId);

        Task ClearStatusAndMatches();
    }
}
