using System.ComponentModel.DataAnnotations;

namespace Buk.UniversalGames.Api;

public class Team {

    [Key]
    public Guid TeamId { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string League { get; set; }

}