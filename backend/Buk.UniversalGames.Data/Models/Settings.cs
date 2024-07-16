using System.ComponentModel.DataAnnotations;

namespace Buk.UniversalGames.Data.Models;

public class Settings
{
    [Key]
    public required string Key { get; set; }

    public required string Value { get; set; }
}