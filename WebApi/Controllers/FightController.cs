namespace OppgaveUkeEnModul3.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OppgaveUkeEnModul3.Core;
using OppgaveUkeEnModul3.WebApi.Services;

[ApiController]
[Route("/[controller]")]
public class FightController(GameService gameService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> StartFight(StartFightDTO dto)
    {
        var result = await gameService.StartFightAsync(
            dto.CharacterId,
            dto.MonsterId);

        return Ok(result);
    }
}