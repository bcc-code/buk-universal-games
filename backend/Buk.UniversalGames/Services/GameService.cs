using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Exceptions;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<List<Game>> GetGames()
        {
            return await _gameRepository.GetGames();
        }

        public async Task<List<MatchListItem>> GetMatches(Team team)
        {
            return await _gameRepository.GetMatches(team);
        }

        public async Task<List<MatchListItem>> GetGameMatches(int leagueId, int? gameId = null)
        {
            return await _gameRepository.GetMatches(leagueId, gameId);
        }

        public async Task<MatchWinnerResult> SetMatchWinner(int matchId, int winnerTeamId)
        {
            var team = await _leagueRepository.GetTeam(winnerTeamId) ?? throw new BadRequestException(Strings.UnknownTeam);
            var match = (await GetMatches(team)).FirstOrDefault(s=>s.MatchId == matchId) ?? throw new BadRequestException(Strings.UnknownMatchId);
            var game = (await GetGames()).FirstOrDefault(s => s.GameId == match.GameId) ?? throw new BadRequestException(Strings.UnknownGame);
            return await _gameRepository.SetMatchWinner(game, matchId, team);
        }

        public async Task<byte[]> GetMatchExport()
        {
            using var stream = new MemoryStream();

            var xlsWorkbook = new XSSFWorkbook();

            var leagues = await _leagueRepository.GetLeagues();
            var gamesIndexedById = (await _gameRepository.GetGames()).ToDictionary(s => s.GameId);

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

                var matches = (await _gameRepository.GetMatches(league.LeagueId)).OrderBy(s => s.Start)
                    .ThenBy(s => s.GameId).ToList();

                rowIndex++;

                foreach (var match in matches)
                {
                    var game = gamesIndexedById[match.GameId];

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

        public Task<ActionResult<MatchWinnerResult>> FinishMatch(MatchResultDto matchResult)
        {
            throw new NotImplementedException();
        }
    }
}
