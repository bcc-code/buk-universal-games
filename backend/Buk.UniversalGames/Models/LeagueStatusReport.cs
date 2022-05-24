using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Models
{
    public class LeagueStatusReport
    {
        public DateTime StatusAt { get; set; }
        public List<TeamStatus> Status { get; set; }
    }
}
