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

    public required string Name { get; set; }

    public required string Code { get; set; }

    public string? Color { get; set; }


    public virtual TeamType Type => TeamType switch
    {
        "participant" => Library.Enums.TeamType.Participant,
        "admin" => Library.Enums.TeamType.Admin,
        "system_admin" => Library.Enums.TeamType.SystemAdmin,
        _ => Library.Enums.TeamType.Participant
    };

    public int MemberCount { get; set; }

    [Column("type")]
    public required string TeamType { get; set; }


    public int? LeagueId { get; set; }
    [JsonIgnore]
    public League? League { get; set; }

    public int? FamilyId { get; set; }
    [JsonIgnore]
    public Family? Family { get; set; }
    [JsonIgnore]

    public required List<PointsRegistration> Points { get; set; }
    [JsonIgnore]
    public List<StickerScan>? StickerScans { get; set; }
}
