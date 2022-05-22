using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface IStatusRepository
    {
        List<Point> GetTeamPoints(Team team);

        List<Point> GetLeaguePoints(int leagueId);
    }
}
