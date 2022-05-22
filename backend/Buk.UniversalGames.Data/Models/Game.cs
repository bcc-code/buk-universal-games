using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Buk.UniversalGames.Library.Enums;

namespace Buk.UniversalGames.Data.Models;

public class Game {

    [Key]
    [JsonPropertyName("id")]
    public int GameId { get; set; }

    public string Name{ get; set; }

    [JsonIgnore]
    public GameType Type{ get; set; }

    public virtual string GameType => Type.ToString();

    public int WinnerPoints{ get; set; }

    public int LooserPoints { get; set; }
}