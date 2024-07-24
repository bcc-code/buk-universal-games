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

    public class TeamTypeFilter : IAsyncAuthorizationFilter
    {
        readonly TeamType[] _authorizedTeamTypes;
        readonly ILeagueService _leagueService;

        public TeamTypeFilter(TeamType[] types, ILeagueService leagueService)
        {
            _authorizedTeamTypes = types;
            _leagueService = leagueService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Console.WriteLine("Entering OnAuthorizationAsync method.");

            if (_authorizedTeamTypes.Length > 0)
            {
                string? code = default;
                if (context.HttpContext.Request.Headers.TryGetValue("x-ubg-teamcode", out var codeFromHeader))
                {
                    code = codeFromHeader;
                    Console.WriteLine($"Team code found in headers: {code}");
                }
                else if (context.RouteData.Values.TryGetValue("code", out var codeFromRoute))
                {
                    code = codeFromRoute?.ToString();
                    Console.WriteLine($"Team code found in route data: {code}");
                }

                if (string.IsNullOrEmpty(code))
                {
                    Console.WriteLine("Team code is missing.");
                    context.Result = new ExceptionResult(Strings.MissingTeamCode, 401);
                }
                else
                {
                    var team = await _leagueService.GetTeamByCode(code!);
                    if (team is null)
                    {
                        Console.WriteLine($"Unknown team code: {code}");
                        context.Result = new ExceptionResult(Strings.UnknownTeamCode, 401);
                    }
                    else if (!_authorizedTeamTypes.Contains(team.Type))
                    {
                        Console.WriteLine($"Team unauthorized. Team type: {team.Type}");
                        context.Result = new ExceptionResult(Strings.TeamUnathorized, 403);
                    }
                    else
                    {
                        Console.WriteLine($"Team authorized: {team.TeamId}");
                        context.HttpContext.Items["ValidatedTeam"] = team;
                    }
                }
            }
            Console.WriteLine("Exiting OnAuthorizationAsync method.");
        }

    }

}
