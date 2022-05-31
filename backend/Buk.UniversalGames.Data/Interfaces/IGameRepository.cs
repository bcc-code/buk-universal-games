using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface IGameRepository
    {
        List<Game> GetGames();

        List<MatchListItem> GetMatches(Team team);
    }
}
