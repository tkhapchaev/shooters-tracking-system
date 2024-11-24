using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dto;
using ShootersTrackingSystem.Model.Entities;

namespace ShootersTrackingSystem.Controllers;

[Route("api/weapons")]
[ApiController]
public class WeaponsController : ControllerBase
{
    private readonly ShootersDbContext _shootersDbContext;

    public WeaponsController(ShootersDbContext shootersDbContext)
    {
        _shootersDbContext = shootersDbContext;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Instructor,Client")]
    public async Task<ActionResult<IEnumerable<Weapon>>> GetWeapons()
    {
        return await _shootersDbContext.Weapons.Include(weapon => weapon.WeaponType).ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Weapon>> CreateWeapon(WeaponDto weaponDto)
    {
        var weapon = await _shootersDbContext.Weapons.FirstOrDefaultAsync(w => w.Name == weaponDto.Name);

        if (weapon is not null)
        {
            return BadRequest();
        }
        
        weapon = new Weapon
        {
            Name = weaponDto.Name,
            WeaponTypeId = weaponDto.WeaponTypeId
        };

        try
        {
            _shootersDbContext.Weapons.Add(weapon);
            await _shootersDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return CreatedAtAction(nameof(GetWeapons), new { id = weapon.Id }, weapon);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<Weapon>> GetWeapon(int id)
    {
        var weapon = await _shootersDbContext.Weapons.Include(w => w.WeaponType).FirstOrDefaultAsync(w => w.Id == id);

        if (weapon is null)
        {
            return NotFound();
        }

        return weapon;
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateWeapon(int id, WeaponDto weaponDto)
    {
        var weapon = await _shootersDbContext.Weapons.FindAsync(id);
        
        if (weapon is null)
        {
            return NotFound();
        }

        weapon.Name = weaponDto.Name;
        weapon.WeaponTypeId = weaponDto.WeaponTypeId;

        try
        {
            _shootersDbContext.Entry(weapon).State = EntityState.Modified;
            await _shootersDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            if (!WeaponExists(id))
            {
                return NotFound();
            }
            
            return BadRequest(e.Message);
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteWeapon(int id)
    {
        var weapon = await _shootersDbContext.Weapons.FindAsync(id);
        
        if (weapon is null)
        {
            return NotFound();
        }

        _shootersDbContext.Weapons.Remove(weapon);
        await _shootersDbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool WeaponExists(int id)
    {
        return _shootersDbContext.Weapons.Any(weapon => weapon.Id == id);
    }
}