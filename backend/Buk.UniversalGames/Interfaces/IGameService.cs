using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Interfaces
{
    public interface IGameService
    {
        List<Game> GetGames();

        List<MatchListItem> GetMatches(Team team);
    }
}
