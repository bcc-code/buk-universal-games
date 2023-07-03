namespace Buk.UniversalGames.Api.Dto
{
    public class GuessDto
    {
        public required int QuestionId { get; init; }
        public required string Answer { get; init; }

        public required string Coin { get; init; }
        public DateTime Time { get; init; }
    }
}
