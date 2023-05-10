using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buk.UniversalGames.Data.Models.Matches
{
    public class MatchResult : IMatchResult
    {
        internal MatchResult(MatchResultUnit unit, Match match, Team team, int measuredResult)
        {
            Unit = unit;
            Match = match;
            Team = team;
            MeasuredResult = measuredResult;
        }

        public MatchResultUnit Unit { get; }
        public Match Match { get; }
        public Team Team { get; }
        public int MeasuredResult { get; }
    }
}
