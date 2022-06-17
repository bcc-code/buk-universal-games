using Buk.UniversalGames.Data.Exceptions;
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

        public ScanResult ScanSticker(Team team, string code)
        {
            var sticker = _stickerRepository.GetStickerByCode(code);
            if (sticker == null)
                throw new BadRequestException(Strings.UnknownStickerCode);

            if (team.LeagueId.GetValueOrDefault() != sticker.LeagueId)
                throw new BadRequestException(Strings.WrongLeagueSticker);

            var message = "";

            try
            {
                var scanResult = _stickerRepository.LogStickerScanning(team, sticker);

                // NB! If scan log will be part of cache - this is success scan log item
                var scan = scanResult.Scan;

                // NB! If points will be part of cache - this is added points after scanning
                var point = scanResult.Point;

                var responseValues = Enum.GetValues(typeof(StickerScanSuccessResponses));
                var responseType = (StickerScanSuccessResponses)responseValues.GetValue(new Random().Next(responseValues.Length))!;

                switch (responseType)
                {
                    case StickerScanSuccessResponses.PointsAdded:
                        message = Strings.ScanSuccessPointsInfo.Replace("{points}", sticker.Points.ToString());
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
                    case StickerScanScannedResponses.SinceScanned:
                        var timespan = DateTime.Now - ex.ScanCheckResult.LastScan;
                        var minutes = (int)Math.Ceiling(timespan.TotalMinutes);
                        if (minutes > 1)
                            message = Strings.ScanErrorScannedLastTime.Replace("{time}", minutes.ToString()).Replace("{unit}", Strings.Minutes);
                        else
                        {
                            var seconds = (int)Math.Ceiling(timespan.TotalSeconds);
                            message = Strings.ScanErrorScannedLastTime.Replace("{time}", seconds.ToString()).Replace("{unit}", Strings.Seconds);
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

        public List<Sticker> GetStickers(int leagueId)
        {
            return _stickerRepository.GetStickers(leagueId);
        }

        public byte[]? GetStickerQR(string stickerCode, int size = 20)
        {
            var sticker = _stickerRepository.GetSticker(stickerCode);
            return sticker != null ? StickerHelper.GetQRImage(sticker.Code, size) : null;
        }

        public byte[] ExportStickers()
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

                    var stickers = _stickerRepository.GetStickers(league.LeagueId);

                    rowIndex++;
                    foreach (var sticker in stickers)
                    {
                        row = xlsSheet.CreateRow(rowIndex);
                        row.CreateCell(0).SetCellValue(league.Name);
                        row.CreateCell(1).SetCellValue(sticker.Code);
                        row.CreateCell(2).SetCellValue(StickerHelper.GetStickerLink(sticker.Code));
                        row.CreateCell(3).SetCellValue(StickerHelper.GetStickerQRLInk(sticker.Code, 20));

                        rowIndex++;
                    }
                }

                xlsWorkbook.Write(stream);
                return stream.ToArray();
            }
        }
    }
}
