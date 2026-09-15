namespace OppgaveUkeEnModul3.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using OppgaveUkeEnModul3.Core;
using Microsoft.EntityFrameworkCore;
using OppgaveUkeEnModul3.WebApi.DatabaseContext;

[ApiController]
[Route("/[controller]")]
public class StoreCharactersController : ControllerBase
{
    private readonly StoreMonstersContext database;

    public StoreCharactersController(StoreMonstersContext database)
    {
        this.database = database;
    }

    [HttpPut("{characterId:guid}/equipment/{swordId:guid}")]
    public async Task<IActionResult> EquipSword(
    Guid characterId,
    Guid swordId)
    {
        var character = await database.Characters
            .FindAsync(characterId);

        if (character is null)
            return NotFound("Character not found.");

        var sword = await database.Swords
            .FindAsync(swordId);

        if (sword is null)
            return NotFound("Sword not found.");

        character.SwordId = sword.Id;

        await database.SaveChangesAsync();

        return Ok(character);
    }
    [HttpGet]
    public async Task<ActionResult<List<StoreCharacter>>> Get()
    {
        var characters = await database.Characters.AsNoTracking().ToListAsync();
        return Ok(characters);
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var character = database.Characters.FirstOrDefault(
            character => character.Id == id);

        return character is null
            ? NotFound()
            : Ok(character);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateCharacterDTO dto)
    {
        var character = new StoreCharacter
        {
            Name = dto.Name
        };

        database.Characters.Add(character);
        await database.SaveChangesAsync();

        return Created(
            $"/StoreCharacters/{character.Id}",
            character);
    }
    [HttpPost("{id:guid}/camp")]
    public async Task<IActionResult> Camp(Guid id)
    {
        var character = await database.Characters.FindAsync(id);

        if (character is null)
            return NotFound("Character not found.");

        var roundsSinceCamp =
            character.Round - character.LastCampRound;

        if (roundsSinceCamp < 3)
        {
            var roundsLeft = 3 - roundsSinceCamp;

            return BadRequest(
                $"You must complete {roundsLeft} more round(s) before camping.");
        }

        character.Hp = character.MaxHp;
        character.LastCampRound = character.Round;

        await database.SaveChangesAsync();

        return Ok(new
        {
            message = $"You rested at camp and restored your HP to {character.MaxHp}.",
            character.Name,
            character.Hp,
            character.Round
        });
    }
}