using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Buk.UniversalGames.Data.Models.Matches;

public class Match
{

    [Key]
    [JsonPropertyName("id")]
    public int MatchId { get; set; }

    public int Team1Id { get; set; }

    public int Team2Id { get; set; }
    public int GameId { get; set; }

    public int LeagueId { get; set; }

    public required string AddOn { get; set; }

    public DateTime Start { get; set; }

    public int? WinnerId { get; set; }

    public required string Position { get; set; }

    [JsonIgnore]
    public required Team Team1 { get; set; }
    [JsonIgnore]
    public required Team Team2 { get; set; }
    [JsonIgnore]
    public required Game Game { get; set; }
    [JsonIgnore]
    public Team? Winner { get; set; }

    public required League League { get; set; }
}