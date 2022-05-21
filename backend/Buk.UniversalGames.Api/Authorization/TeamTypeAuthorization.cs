using System.Security.Claims;
using Buk.UniversalGames.Data.Interfaces;
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
        readonly ITeamRepository _teamRepository;

        public TeamTypeFilter(TeamType[] types, ITeamRepository teamRepository)
        {
            _types = types;
            _teamRepository = teamRepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_types.Length > 0)
            {
                var code = (context.RouteData.Values["code"] ?? "").ToString();
                if (string.IsNullOrEmpty(code))
                {
                    context.Result = new UnauthorizedResult("No team code specified", 403);
                }
                else
                {
                    var user = _teamRepository.GetTeamByCode(code);
                    if(user == null)
                        context.Result = new UnauthorizedResult("Unknown team code", 403);
                    else if(!_types.Contains(user.Type))
                        context.Result = new UnauthorizedResult("Team unauthorized for this request", 403);
                }
            }
        }
    }

}
