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
    public static List<Question> Questions = new List<Question>
    {
        new Question(1, "What is the sound?", "shoe brushing"),
        new Question(2, "Who sung this song?", "Dag Helge Bernardsen")
    };
    //private readonly IStickerService _stickerService;

    public SideQuestController(DataContext db) 
    {
        _db = db;
    }

    [Participant]
    [HttpPost("/questions/{questionId}/guesses")]
    public async Task<ActionResult<ScanResult>> Guess(int questionId, [FromBody] GuessDto guess)
    {
        var team = HttpContext.Items["ValidatedTeam"] as Team;
        var addedEntry =_db.Guesses.Add(new Guess
                            { 
                                Answer = guess.Answer,
                                QuestionId = questionId,
                                Team = team!,
                                Time = DateTime.Now,
                                UniqueId = ""
                            }
        );
        await _db.SaveChangesAsync();

        return Accepted(addedEntry.Entity);
    }
}
