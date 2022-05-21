using Buk.UniversalGames.Data.Exceptions;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Library.Exceptions;
using Buk.UniversalGames.Models;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class StickerService : IStickerService
    {
        private readonly ILogger<StickerService> _logger;
        private readonly IStickerRepository _stickerRepository;

        public StickerService(ILogger<StickerService> logger, IStickerRepository stickerRepository)
        {
            _logger = logger;
            _stickerRepository = stickerRepository;
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
    }
}
