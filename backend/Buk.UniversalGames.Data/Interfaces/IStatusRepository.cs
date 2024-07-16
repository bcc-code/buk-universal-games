using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface IStatusRepository
    {

        Task<List<TeamStatus>> GetLeagueStatus(int leagueId);

        Task ClearStatus(List<League> leagues);
    }
}
