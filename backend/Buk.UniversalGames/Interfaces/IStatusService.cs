using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Models;

namespace Buk.UniversalGames.Interfaces
{
    public interface IStatusService
    {
        TeamStatusReport GetTeamStatus(Team team);

        LeagueStatusReport GetLeagueStatus(int leagueId);
    }
}
