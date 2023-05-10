using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buk.UniversalGames.Data.Models.Matches
{
    public class TimeMatchResult : MatchResult
    {
        public TimeMatchResult(Match match, Team team, int timeInSeconds) : base(MatchResultUnit.TimeInSeconds, match, team, timeInSeconds)
        {
        }
    }
}
