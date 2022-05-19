using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Buk.UniversalGames.Library.Enums;

namespace Buk.UniversalGames.Data.Models;

public class Team
{
    [Key]
    [JsonPropertyName("id")]
    public int TeamId { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public int? LeagueId { get; set; }

    public TeamType Type { get; set; }

    [JsonIgnore]
    public virtual League? League { get; set; }
    [JsonIgnore]
    public virtual List<Point> Points { get; set; }
    [JsonIgnore]
    public virtual List<StickerScan> StickerScans { get; set; }
}