using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Buk.UniversalGames.Data.Models;

public class League {

    [Key]
    [JsonPropertyName("id")]
    public int LeagueId { get; set; }

    public required string Name{ get; set; }

    public required string Color { get; set; }


    [JsonIgnore]
    public required List<Team> Teams { get; set; }
    [JsonIgnore]
    public required List<Sticker> Stickers { get; set; }
}