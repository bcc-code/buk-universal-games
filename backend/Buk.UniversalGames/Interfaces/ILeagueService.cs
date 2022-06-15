using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Interfaces
{
    public interface ILeagueService
    {
        League? GetLeague(int leagueId);

        List<League> GetLeagues();

        Team? GetTeamByCode(string code);

        List<Team> GetTeams(int leagueId);

        byte[] ExportTeams();

        byte[] ExportStatus();
    }
}
