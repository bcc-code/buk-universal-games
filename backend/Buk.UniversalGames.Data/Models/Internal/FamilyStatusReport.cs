namespace  Buk.UniversalGames.Data.Models.Internal
{
    public class FamilyStatusReport
    {
        public List<FamilyStatus> Families { get; set; } = new();
    }

    public class FamilyStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public List<TeamFamilyStatus> Teams { get; set; } = new();
    }

    public class TeamFamilyStatus
    {
        public int TeamId { get; set; }
        public string Team { get; set; }
        public int Points { get; set; }
    }
}
