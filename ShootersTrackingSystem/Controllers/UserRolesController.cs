using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dto;
using ShootersTrackingSystem.Model.Entities;

namespace ShootersTrackingSystem.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/users/roles")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly ShootersDbContext _shootersDbContext;

    public UserRolesController(ShootersDbContext shootersDbContext)
    {
        _shootersDbContext = shootersDbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRole>>> GetRoles()
    {
        return await _shootersDbContext.UserRoles.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<UserRole>> CreateRole(UserRoleDto userRoleDto)
    {
        var userRole = await _shootersDbContext.UserRoles.FirstOrDefaultAsync(ur => ur.Name == userRoleDto.Name);

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
            _shootersDbContext.UserRoles.Add(userRole);
            await _shootersDbContext.SaveChangesAsync();
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
        var role = await _shootersDbContext.UserRoles.FindAsync(id);

        if (role is null)
        {
            return NotFound();
        }

        return role;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, UserRoleDto userRoleDto)
    {
        var userRole = await _shootersDbContext.UserRoles.FindAsync(id);
        
        if (userRole is null)
        {
            return NotFound();
        }
        
        userRole.Name = userRoleDto.Name;

        try
        {
            _shootersDbContext.Entry(userRole).State = EntityState.Modified;
            await _shootersDbContext.SaveChangesAsync();
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
        var role = await _shootersDbContext.UserRoles.FindAsync(id);
        
        if (role is null)
        {
            return NotFound();
        }

        _shootersDbContext.UserRoles.Remove(role);
        await _shootersDbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool RoleExists(int id)
    {
        return _shootersDbContext.UserRoles.Any(userRole => userRole.Id == id);
    }
}