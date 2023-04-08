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

    public string Color { get; set; }

    public int? LeagueId { get; set; }

    public TeamType Type { get; set; }

    public virtual string TeamType => Type.ToString();

    [JsonIgnore]
    public League? League { get; set; }
    [JsonIgnore]
    public List<PointsRegistration> Points { get; set; }
    [JsonIgnore]
    public List<StickerScan> StickerScans { get; set; }
}