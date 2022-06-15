using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Interfaces
{
    public interface IGameService
    {
        List<Game> GetGames();

        List<MatchListItem> GetMatches(int teamId);

        List<MatchListItem> GetGameMatches(int leagueId, int? gameId = null);

        MatchWinnerResult SetMatchWinner(int matchId, int winnerTeamId);

        byte[] GetMatchExport();
    }
}
