using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Buk.UniversalGames.Data.Models;

public class Family
{
    [Key]
    [JsonPropertyName("id")]
    public int FamilyId { get; set; }

    public required string Name { get; set; }
    
    public required string Color { get; set; }

    [JsonIgnore]
    public required List<Team> Teams { get; set; }
}