using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Interfaces
{
    public interface ILeagueService
    {
        Task<League?> GetLeague(int leagueId);

        Task<List<League>> GetLeagues();

        Task<Team?> GetTeamByCode(string code);

        Task<List<Team>> GetTeams(int leagueId);

        Task<byte[]> ExportTeams();

        Task<byte[]> ExportStatus();
    }
}
