using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface ILeagueRepository
    {
        Task<League?> GetLeague(int leagueId);

        Task<List<League>> GetLeagues();

        Task<Team?> GetTeam(int teamId);

        Task<Team?> GetTeamByCode(string code);

        Task<List<Team>> GetTeams(int leagueId);
    }
}
