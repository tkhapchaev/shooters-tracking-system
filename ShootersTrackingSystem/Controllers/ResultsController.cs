using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Models;

namespace ShootersTrackingSystem.Controllers;

[Route("api/results")]
[ApiController]
public class ResultsController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;

    public ResultsController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet("by-weapon/{weaponId}")]
    public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByWeapon(int weaponId)
    {
        var w = await _databaseContext.Weapons.FindAsync(weaponId);

        if (w is null)
        {
            return NotFound();
        }
        
        var attempts = await _databaseContext.Attempts
            .Where(attempt => attempt.WeaponId == weaponId)
            .ToListAsync();
        
        if (attempts.Count == 0)
        {
            return NoContent();
        }

        var attemptsOrdered = attempts.OrderByDescending(attempt => attempt.Score);
        var results = new List<ResultDto>();
        
        foreach (var attempt in attemptsOrdered)
        {
            var score = attempt.Score;
            var user = await _databaseContext.Users.FindAsync(attempt.UserId);
            var weapon = await _databaseContext.Weapons.FindAsync(attempt.WeaponId);

            if (user is null || weapon is null)
            {
                continue;
            }

            var username = user.Name;
            var weaponName = weapon.Name;
            
            results.Add(new ResultDto { Score = score, Username = username, WeaponName = weaponName });
        }
        
        return Ok(results);
    }
    
    [HttpGet("by-weapon-type/{weaponTypeId}")]
    public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByWeaponType(int weaponTypeId)
    {
        var w = await _databaseContext.WeaponTypes.FindAsync(weaponTypeId);

        if (w is null)
        {
            return NotFound();
        }
        
        var attempts = await _databaseContext.Attempts
            .Where(attempt => attempt.Weapon.WeaponTypeId == weaponTypeId)
            .ToListAsync();
        
        if (attempts.Count == 0)
        {
            return NoContent();
        }

        var attemptsOrdered = attempts.OrderByDescending(attempt => attempt.Score);
        var results = new List<ResultDto>();
        
        foreach (var attempt in attemptsOrdered)
        {
            var score = attempt.Score;
            var user = await _databaseContext.Users.FindAsync(attempt.UserId);
            var weapon = await _databaseContext.Weapons.FindAsync(attempt.WeaponId);

            if (user is null || weapon is null)
            {
                continue;
            }

            var username = user.Name;
            var weaponName = weapon.Name;
            
            results.Add(new ResultDto { Score = score, Username = username, WeaponName = weaponName });
        }
        
        return Ok(results);
    }
    
    [HttpGet("by-user/{userId}")]
    public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByUser(int userId)
    {
        var u = await _databaseContext.Users.FindAsync(userId);

        if (u is null)
        {
            return NotFound();
        }
        
        var attempts = await _databaseContext.Attempts
            .Where(attempt => attempt.UserId == userId)
            .ToListAsync();
        
        if (attempts.Count == 0)
        {
            return NoContent();
        }

        var attemptsOrdered = attempts.OrderByDescending(attempt => attempt.Score);
        var results = new List<ResultDto>();
        
        foreach (var attempt in attemptsOrdered)
        {
            var score = attempt.Score;
            var user = await _databaseContext.Users.FindAsync(attempt.UserId);
            var weapon = await _databaseContext.Weapons.FindAsync(attempt.WeaponId);

            if (user is null || weapon is null)
            {
                continue;
            }

            var username = user.Name;
            var weaponName = weapon.Name;
            
            results.Add(new ResultDto { Score = score, Username = username, WeaponName = weaponName });
        }
        
        return Ok(results);
    }
}