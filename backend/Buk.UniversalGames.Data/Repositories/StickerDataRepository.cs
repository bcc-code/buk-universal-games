using Buk.UniversalGames.Data.Exceptions;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Library.Exceptions;

namespace Buk.UniversalGames.Data.Repositories
{
    public class StickerDataRepository : IStickerRepository
    {
        private readonly DataContext _db;

        public StickerDataRepository(DataContext db)
        {
            _db = db;
        }

        public Sticker? GetStickerByCode(string code)
        {
            return _db.Stickers.FirstOrDefault(s => s.Code == code);
        }

        public StickerScanResult LogStickerScanning(Team team, Sticker sticker)
        {
            var scans = _db.StickerScans
                .Where(s => s.TeamId == team.TeamId && s.StickerId == sticker.StickerId)
                .GroupBy(s => new
                {
                    s.StickerId
                }).Select(s => new StickerScannedBeforeInfo
                {
                    LastScan = s.Max(s => s.At),
                    Scans = s.Count()
                }).FirstOrDefault();

            var scan = new StickerScan
            {
                TeamId = team.TeamId,
                StickerId = sticker.StickerId,
                At = DateTime.Now
            };

            _db.StickerScans.Add(scan);
            _db.SaveChanges();

            if (scans != null)
                throw new ScannedBeforeException(scans, scan);

            var point = new Point
            {
                TeamId = team.TeamId,
                StickerId = sticker.StickerId,
                Points = sticker.Points,
                Added = DateTime.Now,
            };
            _db.Points.Add(point);
            _db.SaveChanges();

            return new StickerScanResult
            {
                Scan = scan,
                Point = point,
            };
        }
    }
}
