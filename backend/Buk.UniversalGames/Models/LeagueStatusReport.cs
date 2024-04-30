using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Models
{
    public class LeagueStatusReport
    {
        public LeagueStatusReport(Dictionary<string, List<TeamStatus>> status)
        {
            Status = status;
        }

        public Dictionary<string, List<TeamStatus>> Status { get; }
    }
}
