using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Exceptions;
using IronXL;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class GameService : IGameService
    {
        private readonly ILogger<GameService> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly ILeagueRepository _leagueRepository;

        public GameService(ILogger<GameService> logger, IGameRepository gameRepository , ILeagueRepository leagueRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _leagueRepository = leagueRepository;
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

        public byte[] GetMatchExport()
        {
            var xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLSX);
            xlsWorkbook.Metadata.Author = "UBG";

            var leagues = _leagueRepository.GetLeagues();
            var games = _gameRepository.GetGames().ToDictionary(s => s.GameId);

            foreach (var league in leagues)
            {
                var xlsSheet = xlsWorkbook.CreateWorkSheet(league.Name);
                xlsSheet["A1"].Value = "ID";
                xlsSheet["A1"].Style.Font.Bold = true;
                xlsSheet["B1"].Value = "Game";
                xlsSheet["B1"].Style.Font.Bold = true;
                xlsSheet["C1"].Value = "Team1";
                xlsSheet["C1"].Style.Font.Bold = true;
                xlsSheet["D1"].Value = "Team2";
                xlsSheet["D1"].Style.Font.Bold = true;
                xlsSheet["E1"].Value = "Start";
                xlsSheet["E1"].Style.Font.Bold = true;
                xlsSheet["F1"].Value = "Winner";
                xlsSheet["F1"].Style.Font.Bold = true;

                var matches = _gameRepository.GetGameMatches(league.LeagueId).OrderBy(s=>s.Start).ThenBy(s=>s.GameId).ToList();

                var index = 2;
                foreach (var match in matches)
                {
                    var game = games[match.GameId];

                    xlsSheet["A" + index].Value = match.MatchId;
                    xlsSheet["B" + index].Value = game.GameType;
                    xlsSheet["C" + index].Value = match.Team1;
                    xlsSheet["D" + index].Value = match.Team2;
                    xlsSheet["E" + index].Value = match.Start;
                    xlsSheet["F" + index].Value = match.Winner;
                    index++;
                }
            }

            return xlsWorkbook.ToByteArray();
        }
    }
}
