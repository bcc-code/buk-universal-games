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

    public string Code { get; set; }

    public int Points{ get; set; }

    [JsonIgnore]
    public League League { get; set; }

    [NotMapped]
    public string Link
    {
        get { return StickerHelper.GetStickerLink(Code); }
    }

}