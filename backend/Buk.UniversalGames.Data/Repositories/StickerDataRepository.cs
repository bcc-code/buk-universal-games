using Buk.UniversalGames.Data.Exceptions;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Microsoft.EntityFrameworkCore;

namespace Buk.UniversalGames.Data.Repositories
{
    public class StickerDataRepository : IStickerRepository
    {
        private readonly DataContext _db;

        public StickerDataRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<Sticker?> GetStickerByCode(string code)
        {
            return await _db.Stickers.FirstOrDefaultAsync(s => s.Code == code);
        }

        public async Task<StickerScanResult> LogStickerScanning(Team team, Sticker sticker)
        {
            var scans = await _db.StickerScans
                .Where(s => s.TeamId == team.TeamId && s.StickerId == sticker.StickerId)
                .GroupBy(s => new
                {
                    s.StickerId
                }).Select(s => new StickerScannedBeforeInfo
                {
                    LastScan = s.Max(s => s.At),
                    Scans = s.Count()
                }).FirstOrDefaultAsync();

            var scan = new StickerScan
            {
                TeamId = team.TeamId,
                StickerId = sticker.StickerId,
                At = DateTime.Now
            };

            await _db.StickerScans.AddAsync(scan);
            await _db.SaveChangesAsync();

            if (scans != null)
                throw new ScannedBeforeException(scans, scan);

            var point = new Point
            {
                TeamId = team.TeamId,
                StickerId = sticker.StickerId,
                Points = sticker.Points,
                Added = DateTime.Now,
            };
            await _db.Points.AddAsync(point);
            await _db.SaveChangesAsync();

            return new StickerScanResult
            {
                Scan = scan,
                Point = point,
            };
        }

        public async Task<List<Sticker>> GetStickers(int leagueId)
        {

            return await _db.Stickers.Where(s => s.LeagueId == leagueId).ToListAsync();
        }

        public async Task SetRandomStickerPoints()
        {
            var random = new Random();

            var stickers = await _db.Stickers.ToListAsync();
            foreach (var sticker in stickers)
            {
                sticker.Points = random.Next(100, 200);
            }

            await _db.SaveChangesAsync();
        }
    }
}
