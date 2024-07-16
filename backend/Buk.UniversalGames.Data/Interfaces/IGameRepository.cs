using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Matches;

public interface IGameRepository
{
    Task<List<Game>> GetGames();
    Task<Match> GetMatch(int matchId);
    Task<List<MatchListItem>> GetMatches(Team team);
    Task<List<MatchListItem>> GetMatches(int leagueId, int? gameId = null);
    Task<MatchListItem> StoreMatchResult(Match match, int teamId, int measuredResult);
    Task<MatchListItem> StoreMatchResults(Match match, int team1Result, int team2Result);
}
