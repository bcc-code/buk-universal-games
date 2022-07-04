using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface IStatusRepository
    {
        TeamStatus? GetTeamStatus(Team team);

        List<TeamStatus> GetLeagueStatus(int leagueId);

        void ClearStatus(List<League> leagues);
    }
}
