using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Exceptions;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class GameService : IGameService
    {
        private readonly ILogger<GameService> _logger;
        private readonly IGameRepository _gameRepository;

        public GameService(ILogger<GameService> logger, IGameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        public List<Game> GetGames()
        {
            return _gameRepository.GetGames();
        }

        public List<MatchListItem> GetMatches(int teamId)
        {
            return _gameRepository.GetMatches(teamId);
        }

        public List<MatchListItem> GetGameMatches(int leagueId, int? gameId = null)
        {
            return _gameRepository.GetGameMatches(leagueId, gameId);
        }

        public MatchWinnerResult SetMatchWinner(int matchId, int winnerTeamId)
        {
            var match = GetMatches(winnerTeamId).FirstOrDefault(s=>s.MatchId == matchId);
            if (match == null)
                throw new BadRequestException(Strings.UnknownMatchId);

            var game = GetGames().FirstOrDefault(s => s.GameId == match.GameId);
            if(game == null)
                throw new BadRequestException(Strings.UnknownGame);

            return _gameRepository.SetMatchWinner(game, matchId, winnerTeamId);
        }
    }
}
