using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Entities;
using ShootersTrackingSystem.Models;

namespace ShootersTrackingSystem.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;

    public UsersController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _databaseContext.Users.Include(user => user.UserRole).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _databaseContext.Users.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Id == id);
        
        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(UserDto userDto)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Name == userDto.Name);

        if (user is not null)
        {
            return BadRequest();
        }
        
        user = new User
        {
            Name = userDto.Name,
            Password = userDto.Password,
            UserRoleId = userDto.UserRoleId
        };
        
        _databaseContext.Users.Add(user);
        await _databaseContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Name == userDto.Name);

        if (user is not null)
        {
            return BadRequest();
        }
        
        user = await _databaseContext.Users.FindAsync(id);
        
        if (user is null)
        {
            return NotFound();
        }
        
        user.Name = userDto.Name;
        user.Password = userDto.Password;
        userDto.UserRoleId = user.UserRoleId;
        
        _databaseContext.Entry(user).State = EntityState.Modified;

        try
        {
            await _databaseContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _databaseContext.Users.FindAsync(id);
        
        if (user is null)
        {
            return NotFound();
        }

        _databaseContext.Users.Remove(user);
        await _databaseContext.SaveChangesAsync();
        
        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _databaseContext.Users.Any(user => user.Id == id);
    }
}
