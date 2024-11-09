using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dtos;
using ShootersTrackingSystem.Model.Entities;

namespace ShootersTrackingSystem.Controllers;

[Route("api/attempts")]
[ApiController]
public class AttemptsController : ControllerBase
{
    private readonly DatabaseRepository _databaseRepository;

    public AttemptsController(DatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attempt>>> GetAttempts()
    {
        return await _databaseRepository.Attempts
            .Include(attempt => attempt.User)
            .Include(attempt => attempt.Weapon)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Attempt>> CreateAttempt(AttemptDto attemptDto)
    {
        var attempt = new Attempt
        {
            UserId = attemptDto.UserId,
            WeaponId = attemptDto.WeaponId,
            Score = attemptDto.Score,
            DateTime = DateTime.UtcNow
        };

        try
        {
            _databaseRepository.Attempts.Add(attempt);
            await _databaseRepository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return CreatedAtAction(nameof(GetAttempt), new { id = attempt.Id }, attempt);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Attempt>> GetAttempt(int id)
    {
        var attempt = await _databaseRepository.Attempts
            .Include(a => a.User)
            .Include(a => a.Weapon)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (attempt is null)
        {
            return NotFound();
        }

        return attempt;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAttempt(int id, AttemptDto attemptDto)
    {
        var attempt = await _databaseRepository.Attempts.FindAsync(id);
    
        if (attempt is null)
        {
            return NotFound();
        }
        
        attempt.UserId = attemptDto.UserId;
        attempt.WeaponId = attemptDto.WeaponId;
        attempt.Score = attemptDto.Score;

        try
        {
            _databaseRepository.Entry(attempt).State = EntityState.Modified;
            await _databaseRepository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            if (!AttemptExists(id))
            {
                return NotFound();
            }
            
            return BadRequest(e.Message);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttempt(int id)
    {
        var attempt = await _databaseRepository.Attempts.FindAsync(id);
        
        if (attempt is null)
        {
            return NotFound();
        }

        _databaseRepository.Attempts.Remove(attempt);
        await _databaseRepository.SaveChangesAsync();

        return NoContent();
    }

    private bool AttemptExists(int id)
    {
        return _databaseRepository.Attempts.Any(attempt => attempt.Id == id);
    }
}