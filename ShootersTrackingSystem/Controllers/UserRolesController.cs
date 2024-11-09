using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dtos;
using ShootersTrackingSystem.Model.Entities;

namespace ShootersTrackingSystem.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/users/roles")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly DatabaseRepository _databaseRepository;

    public UserRolesController(DatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRole>>> GetRoles()
    {
        return await _databaseRepository.UserRoles.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<UserRole>> CreateRole(UserRoleDto userRoleDto)
    {
        var userRole = await _databaseRepository.UserRoles.FirstOrDefaultAsync(ur => ur.Name == userRoleDto.Name);

        if (userRole is not null)
        {
            return BadRequest();
        }
        
        userRole = new UserRole
        {
            Name = userRoleDto.Name
        };

        try
        {
            _databaseRepository.UserRoles.Add(userRole);
            await _databaseRepository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return CreatedAtAction(nameof(GetRoles), new { id = userRole.Id }, userRole);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserRole>> GetRole(int id)
    {
        var role = await _databaseRepository.UserRoles.FindAsync(id);

        if (role is null)
        {
            return NotFound();
        }

        return role;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, UserRoleDto userRoleDto)
    {
        var userRole = await _databaseRepository.UserRoles.FindAsync(id);
        
        if (userRole is null)
        {
            return NotFound();
        }
        
        userRole.Name = userRoleDto.Name;

        try
        {
            _databaseRepository.Entry(userRole).State = EntityState.Modified;
            await _databaseRepository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            if (!RoleExists(id))
            {
                return NotFound();
            }

            return BadRequest(e.Message);
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _databaseRepository.UserRoles.FindAsync(id);
        
        if (role is null)
        {
            return NotFound();
        }

        _databaseRepository.UserRoles.Remove(role);
        await _databaseRepository.SaveChangesAsync();

        return NoContent();
    }

    private bool RoleExists(int id)
    {
        return _databaseRepository.UserRoles.Any(userRole => userRole.Id == id);
    }
}