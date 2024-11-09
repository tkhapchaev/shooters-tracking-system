using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dtos;
using ShootersTrackingSystem.Model.Entities;

namespace ShootersTrackingSystem.Controllers;

[Route("api/weapons")]
[ApiController]
public class WeaponsController : ControllerBase
{
    private readonly DatabaseRepository _databaseRepository;

    public WeaponsController(DatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Weapon>>> GetWeapons()
    {
        return await _databaseRepository.Weapons.Include(weapon => weapon.WeaponType).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Weapon>> CreateWeapon(WeaponDto weaponDto)
    {
        var weapon = new Weapon
        {
            Name = weaponDto.Name,
            WeaponTypeId = weaponDto.WeaponTypeId
        };

        try
        {
            _databaseRepository.Weapons.Add(weapon);
            await _databaseRepository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return CreatedAtAction(nameof(GetWeapons), new { id = weapon.Id }, weapon);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Weapon>> GetWeapon(int id)
    {
        var weapon = await _databaseRepository.Weapons.Include(w => w.WeaponType).FirstOrDefaultAsync(w => w.Id == id);

        if (weapon is null)
        {
            return NotFound();
        }

        return weapon;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWeapon(int id, WeaponDto weaponDto)
    {
        var weapon = await _databaseRepository.Weapons.FindAsync(id);
        
        if (weapon is null)
        {
            return NotFound();
        }

        weapon.Name = weaponDto.Name;
        weapon.WeaponTypeId = weaponDto.WeaponTypeId;

        try
        {
            _databaseRepository.Entry(weapon).State = EntityState.Modified;
            await _databaseRepository.SaveChangesAsync();
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
    public async Task<IActionResult> DeleteWeapon(int id)
    {
        var weapon = await _databaseRepository.Weapons.FindAsync(id);
        
        if (weapon is null)
        {
            return NotFound();
        }

        _databaseRepository.Weapons.Remove(weapon);
        await _databaseRepository.SaveChangesAsync();

        return NoContent();
    }

    private bool WeaponExists(int id)
    {
        return _databaseRepository.Weapons.Any(weapon => weapon.Id == id);
    }
}