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

    public virtual GameType Type => GameType switch
    {
        "nervespiral" => Library.Enums.GameType.NerveSpiral,
        "monkeybars" => Library.Enums.GameType.MonkeyBars,
        "tickettwist" => Library.Enums.GameType.TicketTwist,
        "minefield" => Library.Enums.GameType.MineField,
        "tablesurfing" => Library.Enums.GameType.TableSurfing
    };

    [Column("type")]
    public string GameType { get; set; }

    public string Description { get; set; } = "";

    public string ParticipantsInfo { get; set; } = "";

    public string SafetyInfo { get; set; } = "";

    public int WinnerPoints{ get; set; }

    public int LooserPoints { get; set; }
}