using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dtos;
using ShootersTrackingSystem.Model.Entities;

namespace ShootersTrackingSystem.Controllers;

[Route("api/weapons/types")]
[ApiController]
public class WeaponTypesController : ControllerBase
{
    private readonly DatabaseRepository _databaseRepository;

    public WeaponTypesController(DatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<IEnumerable<WeaponType>>> GetWeaponTypes()
    {
        return await _databaseRepository.WeaponTypes.ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<WeaponType>> CreateWeaponType(WeaponTypeDto weaponTypeDto)
    {
        var weaponType = await _databaseRepository.WeaponTypes.FirstOrDefaultAsync(wt => wt.Name == weaponTypeDto.Name);

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
            _databaseRepository.WeaponTypes.Add(weaponType);
            await _databaseRepository.SaveChangesAsync();
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
        var weaponType = await _databaseRepository.WeaponTypes.FindAsync(id);

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
        var weaponType = await _databaseRepository.WeaponTypes.FindAsync(id);
        
        if (weaponType is null)
        {
            return NotFound();
        }

        weaponType.Name = weaponTypeDto.Name;

        try
        {
            _databaseRepository.Entry(weaponType).State = EntityState.Modified;
            await _databaseRepository.SaveChangesAsync();
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
        var weaponType = await _databaseRepository.WeaponTypes.FindAsync(id);
        
        if (weaponType is null)
        {
            return NotFound();
        }

        _databaseRepository.WeaponTypes.Remove(weaponType);
        await _databaseRepository.SaveChangesAsync();

        return NoContent();
    }
    
    private bool WeaponTypeExists(int id)
    {
        return _databaseRepository.WeaponTypes.Any(weaponType => weaponType.Id == id);
    }
}