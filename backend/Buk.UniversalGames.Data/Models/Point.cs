using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Buk.UniversalGames.Data.Models;

public class Point {

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
    public virtual Team Team { get; set; }
    [JsonIgnore]
    public virtual Game? Game { get; set; }
    [JsonIgnore]
    public virtual Match? Match { get; set; }
    [JsonIgnore]
    public virtual Sticker? Sticker { get; set; }
}