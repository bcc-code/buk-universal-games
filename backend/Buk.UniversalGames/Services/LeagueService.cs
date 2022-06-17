﻿using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Helpers;
using Microsoft.Extensions.Logging;
using NPOI.XSSF.UserModel;

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
            using (var stream = new MemoryStream())
            {
                var xlsWorkbook = new XSSFWorkbook();

                var rowIndex = 0;

                var font = xlsWorkbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.FontName = "Calibri";
                font.IsBold = true;

                var style = xlsWorkbook.CreateCellStyle();
                style.SetFont(font);

                var xlsSheet = xlsWorkbook.CreateSheet("Teams");
                var row = xlsSheet.CreateRow(rowIndex);

                var cell = row.CreateCell(0);
                cell.CellStyle = style;
                cell.SetCellValue("ID");

                cell = row.CreateCell(1);
                cell.CellStyle = style;
                cell.SetCellValue("League");

                cell = row.CreateCell(2);
                cell.CellStyle = style;
                cell.SetCellValue("Team");

                cell = row.CreateCell(3);
                cell.CellStyle = style;
                cell.SetCellValue("Code");

                cell = row.CreateCell(4);
                cell.CellStyle = style;
                cell.SetCellValue("Start Link");

                var leagues = GetLeagues();

                rowIndex++;
                foreach (var league in leagues)
                {
                    var teams = GetTeams(league.LeagueId);

                    foreach (var team in teams)
                    {
                        row = xlsSheet.CreateRow(rowIndex);
                        row.CreateCell(0).SetCellValue(team.TeamId);
                        row.CreateCell(1).SetCellValue(league.Name);
                        row.CreateCell(2).SetCellValue(team.Name);
                        row.CreateCell(3).SetCellValue(team.Code);
                        row.CreateCell(4).SetCellValue(TeamHelper.GetStartLink(team.Code));

                        rowIndex++;
                    }
                }

                xlsWorkbook.Write(stream);
                return stream.ToArray();
            }
        }

        public byte[] ExportStatus()
        {
            using (var stream = new MemoryStream())
            {
                var xlsWorkbook = new XSSFWorkbook();

                var font = xlsWorkbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.FontName = "Calibri";
                font.IsBold = true;

                var style = xlsWorkbook.CreateCellStyle();
                style.SetFont(font);

                var leagues = _leagueRepository.GetLeagues();

                foreach (var league in leagues)
                {
                    var xlsSheet = xlsWorkbook.CreateSheet(league.Name);

                    var rowIndex = 0;
                    var row = xlsSheet.CreateRow(rowIndex);

                    var cell = row.CreateCell(0);
                    cell.CellStyle = style;
                    cell.SetCellValue("Position");

                    cell = row.CreateCell(1);
                    cell.CellStyle = style;
                    cell.SetCellValue("Team");

                    cell = row.CreateCell(2);
                    cell.CellStyle = style;
                    cell.SetCellValue("Points");

                    cell = row.CreateCell(3);
                    cell.CellStyle = style;
                    cell.SetCellValue("Stickers");

                    var statuses = _statusRepository.GetLeagueStatus(league.LeagueId);

                    rowIndex ++;
                    foreach (var status in statuses)
                    {
                        row = xlsSheet.CreateRow(rowIndex);
                        row.CreateCell(0).SetCellValue(rowIndex);
                        row.CreateCell(1).SetCellValue(status.Team);
                        row.CreateCell(2).SetCellValue(status.Points);
                        row.CreateCell(3).SetCellValue(status.Stickers);

                        rowIndex++;
                    }
                }

                xlsWorkbook.Write(stream);
                return stream.ToArray();
            }
        }
    }
}
