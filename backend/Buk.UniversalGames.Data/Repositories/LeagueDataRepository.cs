using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Buk.UniversalGames.Data.Repositories
{
    public class LeagueDataRepository : ILeagueRepository
    {
        private readonly DataContext _db;

        public LeagueDataRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<League?> GetLeague(int leagueId)
        {
            return await _db.Leagues.FirstOrDefaultAsync(s => s.LeagueId == leagueId);
        }

        public async Task<List<League>> GetLeagues()
        {
            return await _db.Leagues.ToListAsync();
        }

        public async Task<Team?> GetTeam(int teamId)
        {
            return await _db.Teams.FirstOrDefaultAsync(s => s.TeamId == teamId);
        }
        public async Task<TeamDto?> GetTeamByCode(string code)
        {
            var team = await _db.Teams.AsTracking()
                .Include(t => t.Family)
                .Include(t => t.League)
                .FirstOrDefaultAsync(s => s.Code == code);

            if (team == null)
                return null;

            return new TeamDto
            {
                TeamId = team.TeamId,
                Name = team.Name,
                Code = team.Code,
                Color = team.Color,
                Type = team.Type,
                MemberCount = team.MemberCount,
                TeamType = team.TeamType,
                LeagueId = team.LeagueId,
                LeagueName = team.League?.Name,
                FamilyId = team.FamilyId,
                FamilyName = team.Family?.Name,
                Points = team.Points,
                StickerScans = team.StickerScans
            };
        }

        public async Task<List<Team>> GetTeams(int leagueId)
        {
            return await _db.Teams.Where(s => s.LeagueId == leagueId).ToListAsync();
        }
    }
}
