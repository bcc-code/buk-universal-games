namespace Buk.UniversalGames.Models
{
    public class TeamStatus
    {
        public int TeamId { get; set; }
        public string Team { get; set; }
        public int Points { get; set; }
        public int Stickers { get; set; }
        public DateTime StatusAt { get; set; }
    }
}
