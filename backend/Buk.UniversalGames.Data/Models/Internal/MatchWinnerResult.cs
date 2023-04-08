namespace Buk.UniversalGames.Data.Models.Internal
{
    public class MatchWinnerResult
    {
        public Match Match { get; set; }
        public PointsRegistration? WinnerPoint{ get; set; }
        public PointsRegistration? LooserPoint { get; set; }
    }
}
