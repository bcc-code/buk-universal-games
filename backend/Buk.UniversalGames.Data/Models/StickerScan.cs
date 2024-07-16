using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Buk.UniversalGames.Data.Models;

public class StickerScan {

    [Key]
    [JsonPropertyName("id")]
    public int StickerScanId{ get; set; }

    public int StickerId { get; set; }

    public int TeamId { get; set; }

    public DateTime At { get; set; }

    [JsonIgnore]
    public required Team Team { get; set; }
    [JsonIgnore]
    public required Sticker Sticker { get; set; }
}