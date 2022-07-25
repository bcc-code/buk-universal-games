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

    public class ParticipantFilter : IAsyncAuthorizationFilter
    {
        readonly ILeagueService _leagueService;

        public ParticipantFilter(ILeagueService leagueService)
        {
            _leagueService = leagueService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var code = (context.RouteData.Values["code"] ?? "").ToString();
            if (string.IsNullOrEmpty(code))
            {
                context.Result = new ExceptionResult(Strings.MissingTeamCode, 403);
            }
            else
            {
                var team = await _leagueService.GetTeamByCode(code);
                if(team == null)
                    context.Result = new ExceptionResult(Strings.UnknownTeamCode, 403);
                else if(team.Type != TeamType.Participant)
                    context.Result = new ExceptionResult(Strings.ParticipantsOnly, 403);

                context.HttpContext.Items["ValidatedTeam"] = team;
            }
        }
    }

}
