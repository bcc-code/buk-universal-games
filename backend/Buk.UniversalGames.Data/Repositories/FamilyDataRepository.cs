using System.Text.Json;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Models.Internal;
using Microsoft.EntityFrameworkCore;

namespace Buk.UniversalGames.Data.Repositories
{
    public interface IFamilyRepository
    {
        Task<FamilyStatusReport> GetFamilyStatus();
    }


    public class FamilyDataRepository : IFamilyRepository
    {
        private readonly DataContext _db;

        public FamilyDataRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<FamilyStatusReport> GetFamilyStatus()
        {
            var familiesWithTeams = await _db.Families
                .Select(family => new FamilyStatus
                {
                    Id = family.FamilyId,
                    Name = family.Name,
                    Color = family.Color,
                    Teams = family.Teams.Select(team => new TeamFamilyStatus
                    {
                        TeamId = team.TeamId,
                        Team = team.Name,
                        Points = team.Points.Sum(p => p.Points)
                    })
                    .OrderByDescending(team => team.Points)
                    .ToList(),
                    Points = family.Teams.SelectMany(team => team.Points).Sum(p => p.Points)
                })
                .OrderByDescending(family => family.Points)
                .ToListAsync();

            return new FamilyStatusReport
            {
                Families = familiesWithTeams
            };
        }
    }
}

