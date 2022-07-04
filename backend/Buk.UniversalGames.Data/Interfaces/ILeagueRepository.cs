using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface ILeagueRepository
    {
        League? GetLeague(int leagueId);

        List<League> GetLeagues();

        Team? GetTeam(int teamId);

        Team? GetTeamByCode(string code);

        List<Team> GetTeams(int leagueId);
    }
}
