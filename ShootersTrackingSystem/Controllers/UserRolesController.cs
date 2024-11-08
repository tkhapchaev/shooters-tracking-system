using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Entities;
using ShootersTrackingSystem.Models;

namespace ShootersTrackingSystem.Controllers;

[Route("api/users/roles")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;

    public UserRolesController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRole>>> GetRoles()
    {
        return await _databaseContext.UserRoles.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<UserRole>> CreateRole(UserRoleDto userRoleDto)
    {
        var userRole = new UserRole
        {
            Name = userRoleDto.Name
        };
        
        _databaseContext.UserRoles.Add(userRole);
        await _databaseContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetRoles), new { id = userRole.Id }, userRole);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserRole>> GetRole(int id)
    {
        var role = await _databaseContext.UserRoles.FindAsync(id);

        if (role is null)
        {
            return NotFound();
        }

        return role;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, UserRoleDto userRoleDto)
    {
        var userRole = await _databaseContext.UserRoles.FindAsync(id);
        
        if (userRole is null)
        {
            return NotFound();
        }
        
        userRole.Name = userRoleDto.Name;

        _databaseContext.Entry(userRole).State = EntityState.Modified;

        try
        {
            await _databaseContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoleExists(id))
            {
                return NotFound();
            }
            
            throw;
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _databaseContext.UserRoles.FindAsync(id);
        
        if (role is null)
        {
            return NotFound();
        }

        _databaseContext.UserRoles.Remove(role);
        await _databaseContext.SaveChangesAsync();

        return NoContent();
    }

    private bool RoleExists(int id)
    {
        return _databaseContext.UserRoles.Any(userRole => userRole.Id == id);
    }
}