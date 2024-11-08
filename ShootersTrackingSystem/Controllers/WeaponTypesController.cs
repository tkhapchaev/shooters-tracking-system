using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Entities;
using ShootersTrackingSystem.Models;

namespace ShootersTrackingSystem.Controllers;

[Route("api/weapons/types")]
[ApiController]
public class WeaponTypesController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;

    public WeaponTypesController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeaponType>>> GetWeaponTypes()
    {
        return await _databaseContext.WeaponTypes.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<WeaponType>> CreateWeaponType(WeaponTypeDto weaponTypeDto)
    {
        var weaponType = new WeaponType
        {
            Name = weaponTypeDto.Name
        };
        
        _databaseContext.WeaponTypes.Add(weaponType);
        await _databaseContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetWeaponTypes), new { id = weaponType.Id }, weaponType);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<WeaponType>> GetWeaponType(int id)
    {
        var weaponType = await _databaseContext.WeaponTypes.FindAsync(id);

        if (weaponType is null)
        {
            return NotFound();
        }

        return weaponType;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWeaponType(int id, WeaponTypeDto weaponTypeDto)
    {
        var weaponType = await _databaseContext.WeaponTypes.FindAsync(id);
        
        if (weaponType is null)
        {
            return NotFound();
        }

        weaponType.Name = weaponTypeDto.Name;
        
        _databaseContext.Entry(weaponType).State = EntityState.Modified;

        try
        {
            await _databaseContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WeaponTypeExists(id))
            {
                return NotFound();
            }
            
            throw;
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWeaponType(int id)
    {
        var weaponType = await _databaseContext.WeaponTypes.FindAsync(id);
        
        if (weaponType is null)
        {
            return NotFound();
        }

        _databaseContext.WeaponTypes.Remove(weaponType);
        await _databaseContext.SaveChangesAsync();

        return NoContent();
    }
    
    private bool WeaponTypeExists(int id)
    {
        return _databaseContext.WeaponTypes.Any(weaponType => weaponType.Id == id);
    }
}