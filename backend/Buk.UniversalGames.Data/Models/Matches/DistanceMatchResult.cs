using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buk.UniversalGames.Data.Models.Matches
{
    public class DistanceMatchResult : MatchResult
    {
        public DistanceMatchResult(Match match, Team team, int distanceInCm) : base(MatchResultUnit.DistanceInCm, match, team, distanceInCm)
        {
        }
    }
}
