using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Buk.UniversalGames.Data.Models;

public class Sticker {

    [Key]
    [JsonPropertyName("id")]
    public int StickerId { get; set; }

    public int LeagueId{ get; set; }

    public string Code { get; set; }

    public int Points{ get; set; }

    [JsonIgnore]
    public League League { get; set; }

}