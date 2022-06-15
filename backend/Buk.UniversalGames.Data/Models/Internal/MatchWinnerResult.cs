namespace Buk.UniversalGames.Data.Models.Internal
{
    public class MatchWinnerResult
    {
        public Match Match { get; set; }
        public Point? WinnerPoint{ get; set; }
        public Point? LooserPoint { get; set; }
    }
}
