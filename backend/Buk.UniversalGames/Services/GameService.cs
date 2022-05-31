using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Interfaces;
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

        public List<MatchListItem> GetMatches(Team team)
        {
            return _gameRepository.GetMatches(team);
        }
    }
}
