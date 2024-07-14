using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Buk.UniversalGames.Data.Models;

public class Family
{
    [Key]
    [JsonPropertyName("id")]
    public int FamilyId { get; set; }

    public string Name { get; set; }
    
    public string Color { get; set; }

    [JsonIgnore]
    public List<Team> Teams { get; set; }
}