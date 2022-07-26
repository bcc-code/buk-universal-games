using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Microsoft.EntityFrameworkCore;

namespace Buk.UniversalGames.Data.Repositories
{
    public class StatusDataRepository : IStatusRepository
    {
        private readonly DataContext _db;

        public StatusDataRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<TeamStatus?> GetTeamStatus(Team team)
        {
            var points = await _db.Points.Where(p => p.TeamId == team.TeamId).ToListAsync();
            return new TeamStatus
            {
                Team = team.Name,
                TeamId = team.TeamId,
                Points = points.Sum(s=>s.Points),
                Stickers = points.Count(s=>s.StickerId != null)
            };
        }

        public async Task<List<TeamStatus>> GetLeagueStatus(int leagueId)
        {
            return await (from team in _db.Teams
                    where team.LeagueId == leagueId
                    join point in _db.Points on team.TeamId equals point.TeamId into teamPoints
                    from teamPoint in teamPoints.DefaultIfEmpty()
                    group teamPoint by new { team.TeamId, team.Name }
                    into grouped
                    select new TeamStatus
                    {
                        TeamId = grouped.Key.TeamId,
                        Team = grouped.Key.Name,
                        Points = grouped.Sum(s => s.Points),
                        Stickers = grouped.Count(s => s.StickerId != null)
                    }
                ).OrderByDescending(s=>s.Points).ThenBy(s=>s.Team).ToListAsync();
        }

        public async Task ClearStatus(List<League> leagues)
        {
            await _db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE points");
            await _db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE sticker_scans");
            await _db.Database.ExecuteSqlRawAsync("UPDATE matches SET winner_id = NULL");
        }
    }
}
