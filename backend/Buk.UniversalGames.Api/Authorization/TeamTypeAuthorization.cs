using Buk.UniversalGames.Api.Exceptions;
using Buk.UniversalGames.Interfaces;
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
                    context.Result = new ExceptionResult("No team code specified", 403);
                }
                else
                {
                    var user = _leagueService.GetTeamByCode(code);
                    if(user == null)
                        context.Result = new ExceptionResult("Unknown team code", 403);
                    else if(!_types.Contains(user.Type))
                        context.Result = new ExceptionResult("Team unauthorized for this request", 403);
                }
            }
        }
    }

}
