using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Buk.UniversalGames.Data.Models;

public class League {

    [Key]
    [JsonPropertyName("id")]
    public int LeagueId { get; set; }

    public string? Name{ get; set; }


    [JsonIgnore]
    public virtual List<Team> Teams { get; set; }
    [JsonIgnore]
    public virtual List<Sticker> Stickers { get; set; }
}