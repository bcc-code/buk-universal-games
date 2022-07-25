using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface IGameRepository
    {
        Task<List<Game>> GetGames();

        Task<List<MatchListItem>> GetMatches(Team team);

        Task<List<MatchListItem>> GetGameMatches(int leagueId, int? gameId = null);
      
        Task<MatchWinnerResult> SetMatchWinner(Game game, int matchId, Team team);
    }
}
