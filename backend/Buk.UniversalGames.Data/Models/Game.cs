using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Buk.UniversalGames.Library.Enums;

namespace Buk.UniversalGames.Data.Models;

public class Game {

    [Key]
    [JsonPropertyName("id")]
    public int GameId { get; set; }

    public string Name { get; set; } = "";

    public virtual GameType Type
    {
        get
        {
            Console.WriteLine($"Game type: {GameType}"); // Log the GameType value

            return GameType switch
            {
                "logic" => Library.Enums.GameType.Logic,
                "strength" => Library.Enums.GameType.Strength,
                "teamwork" => Library.Enums.GameType.Teamwork,
                "human-shuffle" => Library.Enums.GameType.HumanShuffle,
                "reaction" => Library.Enums.GameType.Reaction,
                _ => throw new NotImplementedException("Invalid game type. Please migrate your data manually"),
            };
        }
    }

    [Column("type")]
    public string GameType { get; set; }

    public string Description { get; set; } = "";

    public string ParticipantsInfo { get; set; } = "";

    public string SafetyInfo { get; set; } = "";

    public int WinnerPoints{ get; set; }

    public int LooserPoints { get; set; }
}
