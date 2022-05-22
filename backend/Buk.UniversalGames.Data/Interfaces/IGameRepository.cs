using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface IGameRepository
    {
        List<Game> GetGames();
    }
}
