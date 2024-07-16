namespace Buk.UniversalGames.Data.Models.Matches
{
    public class MatchResultDto : IMatchResult
    {
        public MatchResultDto() { }
        public MatchResultDto(int matchId, int teamId, int result)
        {
            MatchId = matchId;
            TeamId = teamId;
            Result = result;
        }

        public int MatchId { get; init; }

        public int TeamId { get; init; }

        /// <summary>
        /// Score of the provided team for this match. Unit indicated in game definition
        /// </summary>
        public int Result { get; init; }
    }

}
