using System;

namespace Buk.UniversalGames.Data.Models.SideQuest
{
    public class Guess
    {
        public int Id { get; set; }
        public int TeamId { get; set; }

        public required Team Team { get; init; }

        public required int QuestionId { get; init; }

        public required string Answer { get; init; }

        public required string UniqueId { get; init; }

        public required DateTime Time { get; init; }

        public bool IsCorrect { get; init; }
    }
}
