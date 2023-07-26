using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Models;

namespace Buk.UniversalGames.Interfaces
{
    public interface IStatusService
    {
        Task<List<TeamStatus>> UpdateGameRanking(Game game, int leagueId);
        Task<TeamStatusReport> GetTeamStatus(Team team);

        Task<LeagueStatusReport> GetLeagueStatus(int leagueId);

        Task ClearStatusAndMatches();
        Task<List<TeamStatus>> BuildAndCacheLeagueRanking(int leagueId);
        Task<List<TeamStatus>> BuildAndCacheRankingForSidequest(int leagueId);
        Task<List<TeamStatus>> BuildAndCacheRankingForGameInLeague(Game game, int leagueId);
        Task<List<TeamStatus>> GetGameRanking(GameType gameType, int leagueId);
        Task<Dictionary<string, List<TeamStatus>>> GetLeagueRankings(int leagueId);
        Task GuaranteeAnswersInCache();
        Task<byte[]> ExportStatus();
    }
}
