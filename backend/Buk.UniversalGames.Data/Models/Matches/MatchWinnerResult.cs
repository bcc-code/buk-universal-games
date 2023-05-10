namespace Buk.UniversalGames.Data.Models.Matches
{
    public record MatchWinnerResult(Match Match, PointsRegistration? WinnerPoint, PointsRegistration? LooserPoint);
}
