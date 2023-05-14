using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buk.UniversalGames.Data.Models.Matches
{
    public class TeamMatchResult : IMatchResult
    {
        public TeamMatchResult(int matchId, int? measuredResultTeam1, int? measuredResultTeam2)
        {
            MatchId = matchId;
            MeasuredResultTeam1 = measuredResultTeam1;
            MeasuredResultTeam2 = measuredResultTeam2;
        }

        public int MatchId { get; }
        
        /// <summary>
        /// Score of team 1 as indicated in the match definition. Expressed in unit indicated in match definition
        /// </summary>
        public int? MeasuredResultTeam1 { get; }
        public int? MeasuredResultTeam2 { get; }
    }

}
