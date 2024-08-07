﻿namespace Buk.UniversalGames.Data.Models.Internal
{
    public class FamilyStatusReport
    {
        public List<FamilyStatus> Families { get; set; } = new List<FamilyStatus>();
        public MyStatus MyStatus { get; set; } = new MyStatus();

        public bool IsFrozen { get; set; } = false;
    }

    public class MyStatus
    {
        public int TeamPoints { get; set; }
        public int FamilyPoints { get; set; }
    }

    public class FamilyStatus
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Points { get; set; }
        public required string Color { get; set; }
        public required List<TeamFamilyStatus> Teams { get; set; }
    }

    public class TeamFamilyStatus
    {
        public int TeamId { get; set; }
        public required string Team { get; set; }
        public int Points { get; set; }
    }
}

