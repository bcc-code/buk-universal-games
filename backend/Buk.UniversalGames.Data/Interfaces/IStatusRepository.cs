using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface IStatusRepository
    {
        List<Point> GetTeamPoints(Team team);

        List<TeamPoint> GetLeaguePoints(int leagueId);
    }
}
