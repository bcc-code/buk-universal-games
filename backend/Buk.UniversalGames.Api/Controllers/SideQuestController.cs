using Buk.UniversalGames.Api.Authorization;
using Buk.UniversalGames.Api.Dto;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.SideQuest;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Models;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats;

namespace Buk.UniversalGames.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SideQuestController : ControllerBase
{
    private readonly DataContext _db;
    public static List<Question> Questions = new()
    {
        new Question(1, "What is the sound?", "shoe brushing"),
        new Question(2, "Who sung this song?", "Dag Helge Bernardsen")
    };

    public SideQuestController(DataContext db) 
    {
        _db = db;
    }

    [Participant]
    [HttpPost("/sidequest/guesses")]
    public async Task<ActionResult<ScanResult>> Guess([FromBody] GuessDto[] guesses)
    {
        var team = HttpContext.Items["ValidatedTeam"] as Team;
        _db.Guesses.AddRange(
            guesses.Select(guess => new Guess
            {
                Answer = guess.Answer,
                QuestionId = guess.QuestionId,
                TeamId = team!.TeamId,
                TimeAnswered = guess.Time.ToLocalTime(),
                Time = DateTime.Now,
                UniqueId = guess.Coin

            })
        );
        await _db.SaveChangesAsync();

        return Ok();
    }
}
