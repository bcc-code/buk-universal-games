using Microsoft.AspNetCore.Mvc;

namespace Buk.UniversalGames.Api.Exceptions
{
    public class ExceptionError
    {
        public string Error { get; }

        public ExceptionError(string message)
        {
            Error = message;
        }
    }
    public class ExceptionResult : JsonResult
    {
        public ExceptionResult(string message, int statusCode) : base(new ExceptionError(message))
        {
            StatusCode = statusCode;
        }
    }
}
