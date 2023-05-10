using Buk.UniversalGames.Api.Exceptions;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
