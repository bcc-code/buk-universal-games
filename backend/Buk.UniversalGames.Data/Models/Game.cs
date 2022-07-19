using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Buk.UniversalGames.Library.Enums;

namespace Buk.UniversalGames.Data.Models;

public class Game {

    [Key]
    [JsonPropertyName("id")]
    public int GameId { get; set; }

    public string Name { get; set; } = "";

    public GameType Type{ get; set; }

    public virtual string GameType => Type.ToString();

    public string Description { get; set; } = "";

    public string ParticipantsInfo { get; set; } = "";

    public string SafetyInfo { get; set; } = "";

    public int WinnerPoints{ get; set; }

    public int LooserPoints { get; set; }
}