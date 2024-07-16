using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Buk.UniversalGames.Library.Helpers;

namespace Buk.UniversalGames.Data.Models;

public class Sticker {

    [Key]
    [JsonPropertyName("id")]
    public int StickerId { get; set; }

    public int LeagueId{ get; set; }

    public required string Code { get; set; }

    public int Points{ get; set; }

    [JsonIgnore]
    public required League League { get; set; }

    [NotMapped]
    public string Link => LinkHelper.GetStickerLink(Code);
}