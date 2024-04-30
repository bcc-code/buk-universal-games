using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Authorization
{
    public class ParticipantAttribute : TypeFilterAttribute
    {
        public ParticipantAttribute() : base(typeof(ParticipantFilter))
        {
        }
    }

    public class ParticipantFilter : TeamTypeFilter
    {

        public ParticipantFilter(ILeagueService leagueService)
            : base(new[] { TeamType.Participant }, leagueService)
        {
        }
    }

}
