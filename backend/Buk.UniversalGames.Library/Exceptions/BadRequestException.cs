namespace Buk.UniversalGames.Library.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() { }

        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
