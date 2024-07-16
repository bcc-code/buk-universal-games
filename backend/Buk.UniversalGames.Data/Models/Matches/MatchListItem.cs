﻿namespace Buk.UniversalGames.Data.Models.Matches
{
    public class MatchListItem
    {
        public int MatchId { get; set; }
        public int GameId { get; set; }
        public required string AddOn { get; set; }
        public int Team1Id { get; set; }
        public required string Team1 { get; set; }
        public int Team2Id { get; set; }
        public required string Team2 { get; set; }
        public required string Start { get; init; }
        public int WinnerId { get; set; }
        public string? Winner { get; set; }

        public int? Team1Result { get; set; }
        public int? Team2Result { get; set; }

        public required string Position { get; set; }
    }
}
