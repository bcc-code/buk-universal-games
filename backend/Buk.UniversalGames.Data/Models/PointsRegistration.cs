using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Buk.UniversalGames.Data.Models.Matches;

namespace Buk.UniversalGames.Data.Models;

public class PointsRegistration {

    [Key]
    [JsonPropertyName("id")]
    public int PointId { get; set; }

    public int TeamId{ get; set; }

    public int? GameId { get; set; }

    public int? MatchId{ get; set; }

    public int? StickerId { get; set; }

    public int Points { get; set; }

    public DateTime Added { get; set; }

    [JsonIgnore]
    public  Team? Team { get; set; }
    [JsonIgnore]
    public Game? Game { get; set; }
    [JsonIgnore]
    public Match? Match { get; set; }
    [JsonIgnore]
    public Sticker? Sticker { get; set; }
}
