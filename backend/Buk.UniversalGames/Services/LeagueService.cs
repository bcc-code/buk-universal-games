using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Helpers;
using IronXL;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ILogger<LeagueService> _logger;
        private readonly ILeagueRepository _leagueRepository;
        private readonly IStatusRepository _statusRepository;

        public LeagueService(ILogger<LeagueService> logger, ILeagueRepository leagueRepository, IStatusRepository statusRepository)
        {
            _logger = logger;
            _leagueRepository = leagueRepository;
            _statusRepository = statusRepository;
        }

        public League? GetLeague(int leagueId)
        {
            return _leagueRepository.GetLeague(leagueId);
        }

        public List<League> GetLeagues()
        {
            return _leagueRepository.GetLeagues();
        }

        public Team? GetTeamByCode(string code)
        {
            return _leagueRepository.GetTeamByCode(code);
        }

        public List<Team> GetTeams(int leagueId)
        {
            return _leagueRepository.GetTeams(leagueId);
        }

        public byte[] ExportTeams()
        {
            var xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLSX);
            xlsWorkbook.Metadata.Author = "UBG";

            var xlsSheet = xlsWorkbook.CreateWorkSheet("Teams");
            
            xlsSheet["A1"].Value = "ID";
            xlsSheet["A1"].Style.Font.Bold = true;
            xlsSheet["B1"].Value = "League";
            xlsSheet["B1"].Style.Font.Bold = true;
            xlsSheet["C1"].Value = "Team";
            xlsSheet["C1"].Style.Font.Bold = true;
            xlsSheet["D1"].Value = "Code";
            xlsSheet["D1"].Style.Font.Bold = true;
            xlsSheet["E1"].Value = "Start link";
            xlsSheet["E1"].Style.Font.Bold = true;

            var leagues = GetLeagues();

            var index = 2;
            foreach (var league in leagues)
            {
                var teams = GetTeams(league.LeagueId);

                foreach (var team in teams)
                {
                    xlsSheet["A" + index].Value = team.TeamId;
                    xlsSheet["B" + index].Value = league.Name;
                    xlsSheet["C" + index].Value = team.Name;
                    xlsSheet["D" + index].Value = team.Code;
                    xlsSheet["E" + index].Value = TeamHelper.GetStartLink(team.Code);
                    index++;
                }
            }

            return xlsWorkbook.ToByteArray();
        }

        public byte[] ExportStatus()
        {
            var xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLSX);
            xlsWorkbook.Metadata.Author = "UBG";

            var leagues = _leagueRepository.GetLeagues();

            foreach (var league in leagues)
            {
                var xlsSheet = xlsWorkbook.CreateWorkSheet(league.Name);
                xlsSheet["A1"].Value = "Position";
                xlsSheet["A1"].Style.Font.Bold = true;
                xlsSheet["B1"].Value = "Team";
                xlsSheet["B1"].Style.Font.Bold = true;
                xlsSheet["C1"].Value = "Points";
                xlsSheet["C1"].Style.Font.Bold = true;
                xlsSheet["D1"].Value = "Stickers";
                xlsSheet["D1"].Style.Font.Bold = true;

                var statuses = _statusRepository.GetLeagueStatus(league.LeagueId);

                var index = 2;
                foreach (var status in statuses)
                {
                    xlsSheet["A" + index].Value = index - 1;
                    xlsSheet["B" + index].Value = status.Team;
                    xlsSheet["C" + index].Value = status.Points;
                    xlsSheet["D" + index].Value = status.Stickers;
                    index++;
                }
            }

            return xlsWorkbook.ToByteArray();
        }
    }
}
