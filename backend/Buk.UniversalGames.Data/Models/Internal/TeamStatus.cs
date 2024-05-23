namespace Buk.UniversalGames.Data.Models.Internal
{
    public record TeamStatus(int TeamId, string Team, int Points, int? FamilyId)
    {
        public int? Score { get; init; }
    }
}
