using Buk.UniversalGames.Api.Exceptions;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Cultures;
using Buk.UniversalGames.Library.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Buk.UniversalGames.Api.Authorization
{
    public class TeamTypeAttribute : TypeFilterAttribute
    {
        public TeamTypeAttribute(params TeamType[] types) : base(typeof(TeamTypeFilter))
        {
            Arguments = new object[] { types };
        }
    }

    public class TeamTypeFilter : IAuthorizationFilter
    {
        readonly TeamType[] _types;
        readonly ILeagueService _leagueService;

        public TeamTypeFilter(TeamType[] types, ILeagueService leagueService)
        {
            _types = types;
            _leagueService = leagueService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_types.Length > 0)
            {
                var code = (context.RouteData.Values["code"] ?? "").ToString();
                if (string.IsNullOrEmpty(code))
                {
                    context.Result = new ExceptionResult(Strings.MissingTeamCode, 403);
                }
                else
                {
                    var team = _leagueService.GetTeamByCode(code);
                    if(team == null)
                        context.Result = new ExceptionResult(Strings.UnknownTeamCode, 403);
                    else if(!_types.Contains(team.Type))
                        context.Result = new ExceptionResult(Strings.TeamUnathorized, 403);

                    context.HttpContext.Items["ValidatedTeam"] = team;
                }
            }
        }
    }

}
