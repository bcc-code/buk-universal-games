﻿using Buk.UniversalGames.Data.Interfaces;
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


        public async Task<List<TeamStatus>> GetLeagueStatus(int leagueId)
        {
            return await (from team in _db.Teams
                          where team.LeagueId == leagueId && team.FamilyId != null
                          join point in _db.Points on team.TeamId equals point.TeamId into teamPoints
                          from teamPoint in teamPoints.DefaultIfEmpty()
                          group teamPoint by new { team.TeamId, team.Name, team.FamilyId }
                    into grouped
                          select new TeamStatus
                          (grouped.Key.TeamId, grouped.Key.Name, grouped.Sum(s => s.Points), grouped.Key.FamilyId)
                ).OrderByDescending(s => s.Points).ThenBy(s => s.Team).ToListAsync();
        }

        public async Task ClearStatus(List<League> leagues)
        {
            await _db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE points");
            await _db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE guesses");
            await _db.Database.ExecuteSqlRawAsync("UPDATE matches SET winner_id = NULL");
        }
    }
}
