using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Models
{
    public class LeagueStatusReport
    {
        public LeagueStatusReport(DateTime statusAt, Dictionary<string, List<TeamStatus>> status)
        {
            StatusAt = statusAt;
            Status = status;
        }

        public DateTime StatusAt { get; }
        public Dictionary<string, List<TeamStatus>> Status { get; }
    }
}
