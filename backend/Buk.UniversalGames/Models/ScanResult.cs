namespace Buk.UniversalGames.Models
{
    public class ScanResult
    {
        public required string Code { get; set; }
        public bool Success { get; set; }
        public required string Message { get; set; }
        public int Points { get; set; }
    }
}
