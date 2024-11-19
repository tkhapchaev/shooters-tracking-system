using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dto;
using ShootersTrackingSystem.Model.Entities;

namespace ShootersTrackingSystem.Controllers;

[Route("api/weapons/types")]
[ApiController]
public class WeaponTypesController : ControllerBase
{
    private readonly ShootersDbContext _shootersDbContext;

    public WeaponTypesController(ShootersDbContext shootersDbContext)
    {
        _shootersDbContext = shootersDbContext;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<IEnumerable<WeaponType>>> GetWeaponTypes()
    {
        return await _shootersDbContext.WeaponTypes.ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<WeaponType>> CreateWeaponType(WeaponTypeDto weaponTypeDto)
    {
        var weaponType = await _shootersDbContext.WeaponTypes.FirstOrDefaultAsync(wt => wt.Name == weaponTypeDto.Name);

        if (weaponType is not null)
        {
            return BadRequest();
        }
        
        weaponType = new WeaponType
        {
            Name = weaponTypeDto.Name
        };

        try
        {
            _shootersDbContext.WeaponTypes.Add(weaponType);
            await _shootersDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return CreatedAtAction(nameof(GetWeaponTypes), new { id = weaponType.Id }, weaponType);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<WeaponType>> GetWeaponType(int id)
    {
        var weaponType = await _shootersDbContext.WeaponTypes.FindAsync(id);

        if (weaponType is null)
        {
            return NotFound();
        }

        return weaponType;
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateWeaponType(int id, WeaponTypeDto weaponTypeDto)
    {
        var weaponType = await _shootersDbContext.WeaponTypes.FindAsync(id);
        
        if (weaponType is null)
        {
            return NotFound();
        }

        weaponType.Name = weaponTypeDto.Name;

        try
        {
            _shootersDbContext.Entry(weaponType).State = EntityState.Modified;
            await _shootersDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            if (!WeaponTypeExists(id))
            {
                return NotFound();
            }
            
            return BadRequest(e.Message);
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteWeaponType(int id)
    {
        var weaponType = await _shootersDbContext.WeaponTypes.FindAsync(id);
        
        if (weaponType is null)
        {
            return NotFound();
        }

        _shootersDbContext.WeaponTypes.Remove(weaponType);
        await _shootersDbContext.SaveChangesAsync();

        return NoContent();
    }
    
    private bool WeaponTypeExists(int id)
    {
        return _shootersDbContext.WeaponTypes.Any(weaponType => weaponType.Id == id);
    }
}