using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Models;

namespace Buk.UniversalGames.Interfaces
{
    public interface IStatusService
    {
        TeamStatus GetTeamStatus(Team team);

        List<TeamStatus> GetLeagueStatus(int leagueId);
    }
}
