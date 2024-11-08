using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Entities;
using ShootersTrackingSystem.Models;

namespace ShootersTrackingSystem.Controllers;

[Route("api/attempts")]
[ApiController]
public class AttemptsController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;

    public AttemptsController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attempt>>> GetAttempts()
    {
        return await _databaseContext.Attempts
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
        
        _databaseContext.Attempts.Add(attempt);
        await _databaseContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetAttempt), new { id = attempt.Id }, attempt);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Attempt>> GetAttempt(int id)
    {
        var attempt = await _databaseContext.Attempts
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
        var attempt = await _databaseContext.Attempts.FindAsync(id);
    
        if (attempt is null)
        {
            return NotFound();
        }
        
        attempt.UserId = attemptDto.UserId;
        attempt.WeaponId = attemptDto.WeaponId;
        attempt.Score = attemptDto.Score;
        
        _databaseContext.Entry(attempt).State = EntityState.Modified;

        try
        {
            await _databaseContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AttemptExists(id))
            {
                return NotFound();
            }
            
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttempt(int id)
    {
        var attempt = await _databaseContext.Attempts.FindAsync(id);
        
        if (attempt is null)
        {
            return NotFound();
        }

        _databaseContext.Attempts.Remove(attempt);
        await _databaseContext.SaveChangesAsync();

        return NoContent();
    }

    private bool AttemptExists(int id)
    {
        return _databaseContext.Attempts.Any(attempt => attempt.Id == id);
    }
}
