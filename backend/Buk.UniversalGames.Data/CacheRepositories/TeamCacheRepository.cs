using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class TeamCacheRepository : ITeamRepository
    {
        private readonly ILogger<TeamCacheRepository> _logger;
        private readonly TeamDataRepository _teamRepository;

        public TeamCacheRepository(ILogger<TeamCacheRepository> logger, DataContext dataContext)
        {
            _logger = logger;
            _teamRepository = new TeamDataRepository(dataContext);
        }

        public Team? GetTeamByCode(string code)
        {
            return _teamRepository.GetTeamByCode(code);
        }
    }
}
