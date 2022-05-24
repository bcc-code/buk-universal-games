using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Models
{
    public class TeamStatusReport
    {
        public DateTime StatusAt { get; set; }
        
        public TeamStatus Status { get; set; }
    }
}
