using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class GameCacheRepository : IGameRepository
    {
        private readonly ILogger<GameCacheRepository> _logger;
        private readonly GameDataRepository _data;
        private readonly ICacheContext _cache;

        public GameCacheRepository(ILogger<GameCacheRepository> logger, DataContext dataContext, ICacheContext cache)
        {
            _logger = logger;
            _data = new GameDataRepository(dataContext);
            _cache = cache;
        }

        public List<Game> GetGames()
        {
            // get from cache
            var cacheKey = "Games";
            var result = _cache.Get<List<Game>>(cacheKey);
            if (result == null)
            {
                // fallback to db and set in cache
                result = _data.GetGames();
                _cache.Set(cacheKey, result);
            }
            return result;
        }

        public List<MatchListItem> GetMatches(Team team)
        {
            return GetGameMatches(team.LeagueId.GetValueOrDefault()).Where(s => s.Team1Id == team.TeamId || s.Team2Id == team.TeamId).ToList();
        }

        public List<MatchListItem> GetGameMatches(int leagueId, int? gameId = null)
        {
            var cacheKey = $"Matches_{leagueId}";
            var matches = _cache.Get<List<MatchListItem>>(cacheKey);
            if (matches == null)
            {
                matches = _data.GetGameMatches(leagueId);
                _cache.Set(cacheKey, matches);
            }

            if (gameId == null)
                return matches;
            return matches.Where(s => s.GameId == gameId.GetValueOrDefault()).ToList();
        }

        public MatchWinnerResult SetMatchWinner(Game game, int matchId, Team team)
        {
            var result = _data.SetMatchWinner(game, matchId, team);

            // clear match lists for league and league status
            _cache.Remove($"Matches_{team.LeagueId.GetValueOrDefault()}");
            _cache.Remove($"LeagueStatus_{team.LeagueId.GetValueOrDefault()}");

            return result;
        }
    }
}
