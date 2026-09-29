using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OppgaveUkeEnModul3.Core;
using OppgaveUkeEnModul3.WebApi.Extensions;
using OppgaveUkeEnModul3.WebApi.Services;

namespace OppgaveUkeEnModul3.WebApi.Controllers;

[ApiController]
[Route("/[controller]")]
[Authorize]
public class FightController(
    GameService gameService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> StartFight(StartFightDTO dto)
    {
        var userId = User.GetUserId();

        if (userId is null)
            return Unauthorized();

        var result = await gameService.StartFightAsync(
            dto.CharacterId,
            dto.MonsterId,
            userId);

        return Ok(result);
    }
}