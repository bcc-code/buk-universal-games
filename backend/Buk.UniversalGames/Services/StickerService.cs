﻿using Buk.UniversalGames.Data.Exceptions;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Library.Exceptions;
using Buk.UniversalGames.Library.Helpers;
using Buk.UniversalGames.Models;
using Microsoft.Extensions.Logging;
using NPOI.XSSF.UserModel;

namespace Buk.UniversalGames.Services
{
    public class StickerService : IStickerService
    {
        private readonly ILogger<StickerService> _logger;
        private readonly IStickerRepository _stickerRepository;
        private readonly ILeagueRepository _leagueRepository;

        public StickerService(ILogger<StickerService> logger, IStickerRepository stickerRepository, ILeagueRepository leagueRepository)
        {
            _logger = logger;
            _stickerRepository = stickerRepository;
            _leagueRepository = leagueRepository;
        }

        public async Task<ScanResult> ScanSticker(Team team, string code)
        {
            var sticker = await _stickerRepository.GetStickerByCode(code);
            if (sticker == null)
                throw new BadRequestException(Strings.UnknownStickerCode);

            if (team.LeagueId.GetValueOrDefault() != sticker.LeagueId)
                throw new BadRequestException(Strings.WrongLeagueSticker);

            var message = "";

            try
            {
                var scanResult = await _stickerRepository.LogStickerScanning(team, sticker);

                // NB! If scan log will be part of cache - this is success scan log item
                var scan = scanResult.Scan;

                // NB! If points will be part of cache - this is added points after scanning
                var point = scanResult.Point;

                var responseValues = Enum.GetValues(typeof(StickerScanSuccessResponses));
                var responseType = (StickerScanSuccessResponses)responseValues.GetValue(new Random().Next(responseValues.Length))!;

                switch (responseType)
                {
                    case StickerScanSuccessResponses.PointsAddedAwesome:
                        message = Strings.ScanSuccessPointsAwesomeInfo.Replace("{points}", sticker.Points.ToString());
                        break;
                    case StickerScanSuccessResponses.PointsAddedNice:
                        message = Strings.ScanSuccessPointsNiceInfo.Replace("{points}", sticker.Points.ToString());
                        break;
                    case StickerScanSuccessResponses.PointsAddedPerfect:
                        message = Strings.ScanSuccessPointsPerfectInfo.Replace("{points}", sticker.Points.ToString());
                        break;
                    case StickerScanSuccessResponses.PointsAddedGreat:
                        message = Strings.ScanSuccessPointsGreatInfo.Replace("{points}", sticker.Points.ToString());
                        break;
                }

                return new ScanResult
                {
                    Code = code,
                    Success = true,
                    Message = message,
                    Points = sticker.Points,
                };
            }
            catch (ScannedBeforeException ex)
            {
                // NB! If scan log will be part of cache - this is failed scan log item
                var scan = ex.Scan;

                var responseValues = Enum.GetValues(typeof(StickerScanScannedResponses));
                var responseType = (StickerScanScannedResponses)responseValues.GetValue(new Random().Next(responseValues.Length))!;
                
                switch (responseType)
                {
                    case StickerScanScannedResponses.ScannedTimes: 
                        message = Strings.ScanErrorScannedBefore.Replace("{times}", ex.ScanCheckResult.Scans.ToString());
                        break;
                    case StickerScanScannedResponses.ScannedTimeTooBad:
                        message = Strings.ScanErrorScannedBeforeTooBad.Replace("{times}", ex.ScanCheckResult.Scans.ToString());
                        break;
                    case StickerScanScannedResponses.SinceScanned:
                    case StickerScanScannedResponses.SinceScanned2:
                        var timespan = DateTime.Now - ex.ScanCheckResult.LastScan;
                        var minutes = (int)Math.Ceiling(timespan.TotalMinutes);

                        message = responseType == StickerScanScannedResponses.SinceScanned
                            ? Strings.ScanErrorScannedLastTime
                            : Strings.ScanErrorScannedLastTime2;

                        if (minutes > 1)
                            message = message.Replace("{time}", minutes.ToString()).Replace("{unit}", Strings.Minutes);
                        else
                        {
                            var seconds = (int)Math.Ceiling(timespan.TotalSeconds);
                            message = message.Replace("{time}", seconds.ToString()).Replace("{unit}", Strings.Seconds);
                        }
                        break;
                }
                return new ScanResult
                {
                    Code = code,
                    Success = false,
                    Message = message,
                    Points = 0,
                };
            }
        }

        public async Task<List<Sticker>> GetStickers(int leagueId)
        {
            return await _stickerRepository.GetStickers(leagueId);
        }

        public async Task<byte[]?> GetStickerQR(string stickerCode, int size = 40)
        {
            var sticker = await _stickerRepository.GetStickerByCode(stickerCode);
            if (sticker == null)
                return null;

            var league = await _leagueRepository.GetLeague(sticker.LeagueId);
            return StickerHelper.GetQRImage(sticker.Code, league?.Color ?? "90,32,39", size);
        }

        public async Task<byte[]> ExportStickers()
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

                var leagues = await _leagueRepository.GetLeagues();

                foreach (var league in leagues)
                {
                    var xlsSheet = xlsWorkbook.CreateSheet(league.Name);

                    var rowIndex = 0;
                    var row = xlsSheet.CreateRow(rowIndex);

                    var cell = row.CreateCell(0);
                    cell.CellStyle = style;
                    cell.SetCellValue("League");

                    cell = row.CreateCell(1);
                    cell.CellStyle = style;
                    cell.SetCellValue("Code");

                    cell = row.CreateCell(2);
                    cell.CellStyle = style;
                    cell.SetCellValue("ScanLink");

                    cell = row.CreateCell(3);
                    cell.CellStyle = style;
                    cell.SetCellValue("QR Image");

                    var stickers = await _stickerRepository.GetStickers(league.LeagueId);

                    rowIndex++;
                    foreach (var sticker in stickers)
                    {
                        row = xlsSheet.CreateRow(rowIndex);
                        row.CreateCell(0).SetCellValue(league.Name);
                        row.CreateCell(1).SetCellValue(sticker.Code);
                        row.CreateCell(2).SetCellValue(LinkHelper.GetStickerLink(sticker.Code));
                        row.CreateCell(3).SetCellValue(LinkHelper.GetStickerQRLInk(sticker.Code));

                        rowIndex++;
                    }
                }

                xlsWorkbook.Write(stream);
                return stream.ToArray();
            }
        }

        public async Task SetRandomStickerPoints()
        {
            await _stickerRepository.SetRandomStickerPoints();
        }
    }
}
