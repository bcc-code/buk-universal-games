using System.Text.Json.Serialization;
using Buk.UniversalGames.Library.Enums;

namespace Buk.UniversalGames.Data.Models
{
    public class TeamDto
    {
        [JsonPropertyName("id")]
        public int TeamId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Color { get; set; }

        public TeamType Type { get; set; }

        public int MemberCount { get; set; }

        public string TeamType { get; set; } = string.Empty;

        public int? LeagueId { get; set; }
        public string? LeagueName { get; set; }

        public int? FamilyId { get; set; }
        public string? FamilyName { get; set; }

        public List<PointsRegistration>? Points { get; set; } = new List<PointsRegistration>();

        public List<StickerScan>? StickerScans { get; set; }
    }
}
