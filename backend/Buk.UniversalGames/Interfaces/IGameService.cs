using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Matches;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Interfaces
{
    public interface IGameService
    {
        Task<List<Game>> GetGames();

        Task<List<MatchListItem>> GetMatches(Team team);

        Task<List<MatchListItem>> GetGameMatches(int leagueId, int? gameId = null);

        Task<MatchWinnerResult> SetMatchWinner(int matchId, int winnerTeamId);

        Task<byte[]> GetMatchExport();
        Task<ActionResult<TeamMatchResult>> ReportTeamMatchResult(int matchId, int teamId, int result);
    }
}
