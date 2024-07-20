using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class GameCacheRepository : IGameRepository
    {
        private readonly ILogger<GameCacheRepository> _logger;
        private readonly GameDataRepository _data;
        private readonly ICacheContext _cache;
        private readonly ValidatingCacheService _validatingCacheService;

        public GameCacheRepository(ILogger<GameCacheRepository> logger, DataContext dataContext, ICacheContext cache, ValidatingCacheService validatingCacheService)
        {
            _logger = logger;
            _data = new GameDataRepository(dataContext);
            _cache = cache;
            _validatingCacheService = validatingCacheService;
        }

        public Task<List<Game>> GetGames()
        {
            return _validatingCacheService.WriteThrough("Games", _data.GetGames);
        }

        public Task<Match> GetMatch(int matchId)
        {
            return _validatingCacheService.WriteThrough("GetMatch_" + matchId, () =>
            {
                return _data.GetMatch(matchId);
            });
        }

        public Task<List<MatchListItem>> GetMatches(Team team)
        {

            return _validatingCacheService.WriteThrough($"Matches_team_{team.TeamId}", () =>
            {
                return _data.GetMatches(team);
            });
        }

        public Task<List<MatchListItem>> GetMatches(int leagueId, int? gameId = null)
        {

            return _validatingCacheService.WriteThrough($"Matches_{leagueId}", () =>
            {
                return _data.GetMatches(leagueId, gameId);
            });
        }

        public async Task<MatchListItem> StoreMatchResult(Match match, int teamId, int measuredResult)
        {
            var teamResult = await _data.StoreMatchResult(match, teamId, measuredResult);

            return teamResult;
        }

        public async Task<MatchListItem> StoreMatchResults(Match match, int team1Result, int team2Result)
        {
            var matchResult = await _data.StoreMatchResults(match, team1Result, team2Result);

            return matchResult;
        }

    }
}
