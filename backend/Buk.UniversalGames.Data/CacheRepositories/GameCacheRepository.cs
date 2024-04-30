﻿using Buk.UniversalGames.Data.Interfaces;
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



        public async Task<List<Game>> GetGames()
        {
            // get from cache
            var cacheKey = "Games";
            var result = await _cache.Get<List<Game>>(cacheKey);
            if (result is null)
            {
                // fallback to db and set in cache
                result = await _data.GetGames();
                await _cache.Set(cacheKey, result);
            }
            return result;
        }

        public Task<Match> GetMatch(int matchId)
        {
            return _data.GetMatch(matchId);
        }

        // shit this should call the underlying repo instead.
        public async Task<List<MatchListItem>> GetMatches(Team team)
        {
            if (team.LeagueId is null) throw new ArgumentException("Team doesn't have a league assigned", nameof(team));

            var cacheKey = $"Matches_team_{team.TeamId}";

            var teamMatches = await _cache.Get<List<MatchListItem>>(cacheKey);
            if (teamMatches == null)
            {
                teamMatches = await _data.GetMatches(team);
                await _cache.Set(cacheKey, teamMatches);
            }


            List<MatchListItem> allMatchesInLeague = await GetMatches(team.LeagueId.Value);

            List<MatchListItem> allMatchesForTeam = allMatchesInLeague.Where(s => s.Team1Id == team.TeamId || s.Team2Id == team.TeamId).ToList();
            return allMatchesForTeam;
        }

        // shit gameid should be passed to repository.Pb
        public async Task<List<MatchListItem>> GetMatches(int leagueId, int? gameId = null)
        {
            var cacheKey = $"Matches_{leagueId}";
            var matches = await _cache.Get<List<MatchListItem>>(cacheKey);
            if (matches == null)
            {
                matches = await _data.GetMatches(leagueId);
                await _cache.Set(cacheKey, matches);
            }

            if (gameId == null)
                return matches;
            return matches.Where(s => s.GameId == gameId.Value).ToList();
        }

        public async Task<MatchWinnerResult> SetMatchWinner(Game game, int matchId, Team team)
        {
            if (team.LeagueId is null) throw new ArgumentException("Team doesn't belong to any league", nameof(team));
            var result = await _data.SetMatchWinner(game, matchId, team);

            // clear match lists for league and league status
            await _cache.Remove($"Matches_{team.LeagueId.Value}");
            await _cache.Remove($"LeagueStatus_{team.LeagueId.Value}");

            return result;
        }

        public async Task<MatchListItem> StoreMatchResult(Match match, int teamId, int measuredResult)
        {
            var teamResult = await _data.StoreMatchResult(match, teamId, measuredResult);
            var leagueId = match.LeagueId;
            await _cache.Remove($"Matches_{leagueId}");
            await _cache.Remove($"LeagueStatus_{leagueId}");

            return teamResult;
        }
    }
}
