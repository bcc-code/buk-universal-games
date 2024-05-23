using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StackExchange.Redis;

namespace Buk.UniversalGames.Services
{
    public class StatusService
    {
        private readonly ILogger<StatusService> _logger;
        private readonly IStatusRepository _statusRepository;
        private readonly ILeagueRepository _leagueLeagueRepository;
        private readonly DataContext _db;
        private readonly IGameRepository _gameRepository;

        public StatusService(ILogger<StatusService> logger, IStatusRepository statusRepository, ILeagueRepository leagueLeagueRepository, DataContext db, IGameRepository gameRepository)
        {
            _logger = logger;
            _statusRepository = statusRepository ?? throw new ArgumentNullException(nameof(statusRepository));
            _leagueLeagueRepository = leagueLeagueRepository;
            _db = db;
            _gameRepository = gameRepository;
        }

        public async Task<Dictionary<string, List<TeamStatus>>> GetLeagueRankings(int leagueId)
        {
            var games = await _gameRepository.GetGames();

            // shit should the key to this be string or gametype? 
            var dict = new Dictionary<string, List<TeamStatus>>();

            foreach (var game in games)
            {
                string gameTypeString = game.GameType.ToLowerInvariant();
                List<TeamStatus> gameRanking = await BuildAndCacheRankingForGameInLeague(game, leagueId);

                dict.Add(gameTypeString, gameRanking);
            }

            List<TeamStatus> teamStatuses = await BuildAndCacheLeagueRanking(leagueId);
            dict.Add("total", teamStatuses);

            return dict;
        }

        public async Task<List<TeamStatus>> GetGameRanking(GameType gameType, int leagueId)
        {
            var games = await _gameRepository.GetGames();
            var game = games.First(g => g.Type == gameType);
            List<TeamStatus> gameRanking = await BuildAndCacheRankingForGameInLeague(game, leagueId);
            return gameRanking ?? new List<TeamStatus>();
        }

        public async Task<List<TeamStatus>> UpdateGameRanking(Game game, int leagueId)
        {
            await BuildAndCacheRankingForGameInLeague(game, leagueId);
            return await BuildAndCacheLeagueRanking(leagueId);
        }

        public async Task<List<TeamStatus>> BuildAndCacheRankingForGameInLeague(Game game, int leagueId)

        {

            var teamScoresQuery = from score in _db.Points
                                  where score.Game == game && score.Match!.LeagueId == leagueId
                                  select new { score.TeamId, score.Team.Name, score.Points, score.Team.FamilyId };

            var teamScores = await teamScoresQuery.ToListAsync();

            return teamScores.Select(score => new TeamStatus(score.TeamId, score.Name, score.Points)).ToList();
        }

        // shit make private and rename
        public async Task<List<TeamStatus>> BuildAndCacheLeagueRanking(int leagueId)
        {
            var teams = await _leagueLeagueRepository.GetTeams(leagueId);

            // shit replace with query to game
            var landWaterBeach = (await GetGameRanking(GameType.LandWaterBeach, leagueId)).ToDictionary(x => x.TeamId);
            var humanShuffleBoard = (await GetGameRanking(GameType.HumanShuffleBoard, leagueId)).ToDictionary(x => x.TeamId);
            var labyrinth = (await GetGameRanking(GameType.Labyrinth, leagueId)).ToDictionary(x => x.TeamId);
            var mastermind = (await GetGameRanking(GameType.Mastermind, leagueId)).ToDictionary(x => x.TeamId);
            var ironGrip = (await GetGameRanking(GameType.IronGrip, leagueId)).ToDictionary(x => x.TeamId);



            var leagueRanking = new List<TeamStatus>();

            foreach (var team in teams)
            {
                var totalPoints = GetPointsForSubRanking(landWaterBeach, team)
                                    + GetPointsForSubRanking(humanShuffleBoard, team)
                                    + GetPointsForSubRanking(labyrinth, team)
                                    + GetPointsForSubRanking(mastermind, team)
                                    + GetPointsForSubRanking(ironGrip, team);

                leagueRanking.Add(new TeamStatus(team.TeamId, team.Name, totalPoints));
            }

            var sortedRanking = leagueRanking.OrderByDescending(x => x.Points).ToList();

            return sortedRanking;

            static int GetPointsForSubRanking(Dictionary<int, TeamStatus> subRanking, Team team)
                => subRanking.TryGetValue(team.TeamId, out var status) ? status.Points : 0;
        }

        public async Task<TeamStatusReport> GetTeamStatus(Team team)
        {
            if (team is null) throw new ArgumentNullException(nameof(team));

            return new TeamStatusReport
            {
                Status = await _statusRepository.GetTeamStatus(team),
                StatusAt = DateTime.Now
            };
        }

        public async Task<LeagueStatusReport> GetLeagueStatus(int leagueId)
        {
            return new LeagueStatusReport(await GetLeagueRankings(leagueId));
        }

        public async Task ClearStatusAndMatches()
        {
            var leagues = await _leagueLeagueRepository.GetLeagues();
            await _statusRepository.ClearStatus(leagues);
        }


        public async Task<byte[]> ExportStatus()
        {
            using var stream = new MemoryStream();
            var xlsWorkbook = new XSSFWorkbook();

            var font = xlsWorkbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "Calibri";
            font.IsBold = true;

            var boldCellStyle = xlsWorkbook.CreateCellStyle();
            boldCellStyle.SetFont(font);

            var leagues = await _leagueLeagueRepository.GetLeagues();

            var mainSheet = xlsWorkbook.CreateSheet("Winners");

            var titleRow = mainSheet.CreateRow(0);
            Cell(titleRow, 3, "B-League", boldCellStyle);
            Cell(titleRow, 5, "U-League", boldCellStyle);
            Cell(titleRow, 7, "K-League", boldCellStyle);

            var overallWinnerRow = mainSheet.CreateRow(1);
            Cell(overallWinnerRow, 1, "Overall winner", boldCellStyle);

            var games = (GameType[])Enum.GetValues(typeof(GameType));

            var gameRowIndex = (GameType game) => game switch
            {
                GameType.LandWaterBeach => 5,
                GameType.IronGrip => 6,
                GameType.Mastermind => 7,
                GameType.Labyrinth => 8,
                GameType.HumanShuffleBoard => 9,
                _ => throw new Exception()
            };

            var gameColumnIndex = (GameType game) => game switch
            {
                GameType.LandWaterBeach => 3,
                GameType.IronGrip => 5,
                GameType.Mastermind => 7,
                GameType.Labyrinth => 9,
                GameType.HumanShuffleBoard => 11,
                _ => throw new Exception()
            };

            var gameRows = games.ToDictionary(x => x, x => mainSheet.CreateRow(gameRowIndex(x)));

            foreach (var league in leagues)
            {
                var status = await GetLeagueRankings(league.LeagueId);

                if (!status["total"].Any()) continue;

                var leagueCol = league.Name switch
                {
                    "B-League" => 3,
                    "U-League" => 5,
                    "K-League" => 7,
                    _ => 8
                };

                var leagueSheet = xlsWorkbook.CreateSheet(league.Name);
                var leagueTitleRow = leagueSheet.CreateRow(0);

                //fill league winner in main sheet
                Cell(overallWinnerRow, leagueCol, status["total"].First().Team);
                Cell(overallWinnerRow, leagueCol + 1, status["total"].First().Points);

                //overall ranking
                Cell(leagueTitleRow, 1, "Overall");

                var rowIndex = 2;
                foreach (var team in status["total"])
                {
                    var row = leagueSheet.CreateRow(rowIndex);
                    Cell(row, 1, team.Team);
                    Cell(row, 2, team.Points);
                    rowIndex++;
                }

                foreach (var game in games)
                {
                    var gameRanking = status[game.ToString().ToLowerInvariant()];

                    if (!gameRanking.Any()) continue;

                    // fill game winner on main sheet
                    Cell(gameRows[game], 1, game.ToString());
                    Cell(gameRows[game], leagueCol, gameRanking.First().Team);
                    Cell(gameRows[game], leagueCol + 1, gameRanking.First().Points);

                    // handle league sheet
                    Cell(leagueTitleRow, gameColumnIndex(game), game.ToString(), boldCellStyle);
                    Cell(leagueTitleRow, gameColumnIndex(game) + 1, $"Score ({game})", boldCellStyle);

                    rowIndex = 2;
                    foreach (var team in gameRanking)
                    {
                        Cell(leagueSheet.GetRow(rowIndex), gameColumnIndex(game), team.Team);
                        Cell(leagueSheet.GetRow(rowIndex), gameColumnIndex(game) + 1, team.Points);
                        rowIndex++;
                    }
                }
            }
            xlsWorkbook.Write(stream);
            return stream.ToArray();
        }

        private static ICell Cell(IRow row, int index, string value, ICellStyle? cellStyle = null)
        {
            var cell = row.CreateCell(index);
            if (cellStyle != null)
                cell.CellStyle = cellStyle;

            cell.SetCellValue(value);
            return cell;
        }

        private static ICell Cell(IRow row, int index, int value, ICellStyle? cellStyle = null)
        {
            var cell = row.CreateCell(index);
            if (cellStyle != null)
                cell.CellStyle = cellStyle;

            cell.SetCellValue(value);
            return cell;
        }
    }
}
