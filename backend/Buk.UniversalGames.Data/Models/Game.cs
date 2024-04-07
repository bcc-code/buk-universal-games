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
                "nervespiral" => Library.Enums.GameType.NerveSpiral,
                "monkeybars" => Library.Enums.GameType.MonkeyBars,
                "tickettwist" => Library.Enums.GameType.TicketTwist,
                "minefield" => Library.Enums.GameType.MineField,
                "tablesurfing" => Library.Enums.GameType.TableSurfing,
                "nerve_spiral" => Library.Enums.GameType.NerveSpiral,
                "monkey_bars" => Library.Enums.GameType.MonkeyBars,
                "ticket_twist" => Library.Enums.GameType.TicketTwist,
                "mine_field" => Library.Enums.GameType.MineField,
                "table_surfing" => Library.Enums.GameType.TableSurfing,
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
