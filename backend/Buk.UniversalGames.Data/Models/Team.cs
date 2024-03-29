using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    public virtual TeamType Type => TeamType switch
    {
        "participant" => Library.Enums.TeamType.Participant,
        "admin" => Library.Enums.TeamType.Admin,
        "systemadmin" => Library.Enums.TeamType.SystemAdmin
    };

    public int MemberCount { get; set; }

    [Column("type")]
    public string TeamType { get; set; }

    [JsonIgnore]
    public League? League { get; set; }
    [JsonIgnore]
    public List<PointsRegistration> Points { get; set; }
    [JsonIgnore]
    public List<StickerScan> StickerScans { get; set; }
}