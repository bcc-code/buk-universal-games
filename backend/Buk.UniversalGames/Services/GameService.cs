using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.XSSF.UserModel;
using StackExchange.Redis;

namespace Buk.UniversalGames.Services
{
    public class GameService
    {
        private readonly ILogger<GameService> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly StatusService _statusService;
        private readonly DataContext _db;
        private readonly ValidatingCacheService _validatingCacheService;

        public GameService(ILogger<GameService> logger, IGameRepository gameRepository, ILeagueRepository leagueRepository, StatusService statusService, DataContext db, ValidatingCacheService validatingCacheService)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _leagueRepository = leagueRepository;
            _statusService = statusService;
            _db = db;
            _validatingCacheService = validatingCacheService;
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

        public async Task<ActionResult<MatchListItem>> ReportTeamMatchResult(int matchId, int teamId, int result)
        {
            var match = _db.Matches
                .AsTracking()
                .Include(x => x.Game)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .SingleOrDefault(x => x.MatchId == matchId)
                ?? throw new ArgumentException("No match found", nameof(matchId));

            var matchResult = await _gameRepository.StoreMatchResult(match, teamId, result);

            return matchResult;
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
    }
}
