using System.ComponentModel.DataAnnotations;

namespace Buk.UniversalGames.Data.Models;

public class Settings
{
    [Key]
    public string Key { get; set; }

    public string Value { get; set; }
}