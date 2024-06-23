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
            return GameType switch
            {
                "landwaterbeach" => Library.Enums.GameType.LandWaterBeach,
                "humanshuffleboard" => Library.Enums.GameType.HumanShuffleBoard,
                "labyrinth" => Library.Enums.GameType.Labyrinth,
                "mastermind" => Library.Enums.GameType.Mastermind,
                "irongrip" => Library.Enums.GameType.IronGrip,
                "land_water_beach" => Library.Enums.GameType.LandWaterBeach,
                "human_shuffleboard" => Library.Enums.GameType.HumanShuffleBoard,
                "iron_grip" => Library.Enums.GameType.IronGrip,
                _ => throw new NotImplementedException($"Invalid game type '{GameType}'. Please migrate your data manually"),
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
