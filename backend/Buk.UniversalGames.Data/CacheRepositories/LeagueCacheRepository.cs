using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class LeagueCacheRepository : ILeagueRepository
    {
        private readonly ILogger<LeagueCacheRepository> _logger;
        private readonly LeagueDataRepository _data;
        private readonly ValidatingCacheService _validatingCacheService;

        public LeagueCacheRepository(ILogger<LeagueCacheRepository> logger, DataContext dataContext, ValidatingCacheService validatingCacheService)
        {
            _logger = logger;
            _data = new LeagueDataRepository(dataContext);
            _validatingCacheService = validatingCacheService;
        }

        private static string LeagueCacheKey(int leagueId) => $"League_{leagueId}";
        private static string LeaguesCacheKey() => "Leagues";
        private static string TeamCacheKey(int teamId) => $"TeamId_{teamId}";
        private static string TeamByCodeCacheKey(string code) => $"Team_{code}";
        private static string TeamsCacheKey(int leagueId) => $"Teams_{leagueId}";

        public async Task<League?> GetLeague(int leagueId)
        {
            var leagues = await GetLeagues();
            var league = leagues.FirstOrDefault(s => s.LeagueId == leagueId);
            return league ?? await _data.GetLeague(leagueId);
        }

        public async Task<List<League>> GetLeagues()
        {
            return await _validatingCacheService.WriteThrough(LeaguesCacheKey(), _data.GetLeagues);
        }

        public async Task<Team?> GetTeam(int teamId)
        {
            return await _validatingCacheService.WriteThrough(TeamCacheKey(teamId), () => _data.GetTeam(teamId));
        }

        public async Task<TeamDto?> GetTeamByCode(string code)
        {
            return await _validatingCacheService.WriteThrough(TeamByCodeCacheKey(code), () => _data.GetTeamByCode(code));
        }

        public async Task<List<Team>> GetTeams(int leagueId)
        {
            return await _validatingCacheService.WriteThrough(TeamsCacheKey(leagueId), () => _data.GetTeams(leagueId));
        }
    }
}
