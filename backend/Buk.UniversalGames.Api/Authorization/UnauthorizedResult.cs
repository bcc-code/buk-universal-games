using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Authorization
{
    public class UnauthorizedError
    {
        public string Error { get; }

        public UnauthorizedError(string message)
        {
            Error = message;
        }
    }
    public class UnauthorizedResult : JsonResult
    {
        public UnauthorizedResult(string message, int statusCode) : base(new UnauthorizedError(message))
        {
            StatusCode = statusCode;
        }
    }
}
