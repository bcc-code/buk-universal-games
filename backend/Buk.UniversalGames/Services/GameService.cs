using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Exceptions;
using Microsoft.Extensions.Logging;
using NPOI.XSSF.UserModel;

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

        public List<MatchListItem> GetMatches(Team team)
        {
            return _gameRepository.GetMatches(team);
        }

        public List<MatchListItem> GetGameMatches(int leagueId, int? gameId = null)
        {
            return _gameRepository.GetGameMatches(leagueId, gameId);
        }

        public MatchWinnerResult SetMatchWinner(int matchId, int winnerTeamId)
        {
            var team = _leagueRepository.GetTeam(winnerTeamId);
            if (team == null)
                throw new BadRequestException(Strings.UnknownTeam);

            var match = GetMatches(team).FirstOrDefault(s=>s.MatchId == matchId);
            if (match == null)
                throw new BadRequestException(Strings.UnknownMatchId);

            var game = GetGames().FirstOrDefault(s => s.GameId == match.GameId);
            if(game == null)
                throw new BadRequestException(Strings.UnknownGame);

            return _gameRepository.SetMatchWinner(game, matchId, team);
        }

        public byte[] GetMatchExport()
        {
            using (var stream = new MemoryStream())
            {
                var xlsWorkbook = new XSSFWorkbook();

                var leagues = _leagueRepository.GetLeagues();
                var games = _gameRepository.GetGames().ToDictionary(s => s.GameId);

                var font = xlsWorkbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.FontName = "Calibri";
                font.IsBold = true;

                var style = xlsWorkbook.CreateCellStyle();
                style.SetFont(font);

                foreach (var league in leagues)
                {
                    var xlsSheet = xlsWorkbook.CreateSheet(league.Name);

                    var rowIndex = 0;
                    var row = xlsSheet.CreateRow(rowIndex);

                    var cell = row.CreateCell(0);
                    cell.CellStyle = style;
                    cell.SetCellValue("ID");

                    cell = row.CreateCell(1);
                    cell.CellStyle = style;
                    cell.SetCellValue("Game");

                    cell = row.CreateCell(2);
                    cell.CellStyle = style;
                    cell.SetCellValue("Team1");

                    cell = row.CreateCell(3);
                    cell.CellStyle = style;
                    cell.SetCellValue("Team2");

                    cell = row.CreateCell(4);
                    cell.CellStyle = style;
                    cell.SetCellValue("Start");

                    cell = row.CreateCell(5);
                    cell.CellStyle = style;
                    cell.SetCellValue("Winner");

                    var matches = _gameRepository.GetGameMatches(league.LeagueId).OrderBy(s => s.Start)
                        .ThenBy(s => s.GameId).ToList();

                    rowIndex++;

                    foreach (var match in matches)
                    {
                        var game = games[match.GameId];

                        row = xlsSheet.CreateRow(rowIndex);
                        row.CreateCell(0).SetCellValue(match.MatchId);
                        row.CreateCell(1).SetCellValue(game.GameType);
                        row.CreateCell(2).SetCellValue(match.Team1);
                        row.CreateCell(3).SetCellValue(match.Team2);
                        row.CreateCell(4).SetCellValue(match.Start);
                        row.CreateCell(5).SetCellValue(match.Winner);

                        rowIndex++;
                    }
                }

                xlsWorkbook.Write(stream);
                return stream.ToArray();
            }
        }
    }
}
