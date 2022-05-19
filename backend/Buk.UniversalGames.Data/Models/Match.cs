using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Buk.UniversalGames.Data.Models;

public class Match {

    [Key]
    [JsonPropertyName("id")]
    public int MatchId { get; set; }

    public int Team1Id{ get; set; }

    public int Team2Id{ get; set; }

    public int GameId{ get; set; }

    public DateTime Start { get; set; }

    public int WinnerId { get; set; }

    [JsonIgnore]
    public virtual Team Team1Team { get; set; }
    [JsonIgnore]
    public virtual Team Team2Team { get; set; }
    [JsonIgnore]
    public virtual Game Game { get; set; }
    [JsonIgnore]
    public virtual Team WinnerTeam { get; set; }
}