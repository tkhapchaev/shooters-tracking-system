using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Entities;
using ShootersTrackingSystem.Models;

namespace ShootersTrackingSystem.Controllers;

[Route("api/weapons")]
[ApiController]
public class WeaponsController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;

    public WeaponsController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Weapon>>> GetWeapons()
    {
        return await _databaseContext.Weapons.Include(weapon => weapon.WeaponType).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Weapon>> CreateWeapon(WeaponDto weaponDto)
    {
        var weapon = new Weapon
        {
            Name = weaponDto.Name,
            WeaponTypeId = weaponDto.WeaponTypeId
        };
        
        _databaseContext.Weapons.Add(weapon);
        await _databaseContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetWeapons), new { id = weapon.Id }, weapon);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Weapon>> GetWeapon(int id)
    {
        var weapon = await _databaseContext.Weapons.Include(w => w.WeaponType).FirstOrDefaultAsync(w => w.Id == id);

        if (weapon is null)
        {
            return NotFound();
        }

        return weapon;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWeapon(int id, WeaponDto weaponDto)
    {
        var weapon = await _databaseContext.Weapons.FindAsync(id);
        
        if (weapon is null)
        {
            return NotFound();
        }

        weapon.Name = weaponDto.Name;
        weapon.WeaponTypeId = weaponDto.WeaponTypeId;
        
        _databaseContext.Entry(weapon).State = EntityState.Modified;

        try
        {
            await _databaseContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WeaponExists(id))
            {
                return NotFound();
            }
            
            throw;
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWeapon(int id)
    {
        var weapon = await _databaseContext.Weapons.FindAsync(id);
        
        if (weapon is null)
        {
            return NotFound();
        }

        _databaseContext.Weapons.Remove(weapon);
        await _databaseContext.SaveChangesAsync();

        return NoContent();
    }

    private bool WeaponExists(int id)
    {
        return _databaseContext.Weapons.Any(weapon => weapon.Id == id);
    }
}