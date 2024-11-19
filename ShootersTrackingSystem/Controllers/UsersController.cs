using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dto;
using ShootersTrackingSystem.Model.Entities;
using ShootersTrackingSystem.Model.Services;

namespace ShootersTrackingSystem.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ShootersDbContext _shootersDbContext;
    private readonly AuthService _authService;

    public UsersController(ShootersDbContext shootersDbContext, AuthService authService)
    {
        _shootersDbContext = shootersDbContext;
        _authService = authService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _shootersDbContext.Users.Include(user => user.UserRole).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _shootersDbContext.Users.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Id == id);
        
        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(UserDto userDto)
    {
        var user = await _shootersDbContext.Users.FirstOrDefaultAsync(u => u.Name == userDto.Name);

        if (user is not null)
        {
            return BadRequest();
        }
        
        user = new User
        {
            Name = userDto.Name,
            Password = _authService.HashPassword(userDto.Password),
            UserRoleId = userDto.UserRoleId
        };

        try
        {
            _shootersDbContext.Users.Add(user);
            await _shootersDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
    {
        var user = await _shootersDbContext.Users.FirstOrDefaultAsync(u => u.Name == userDto.Name);

        if (user is not null)
        {
            return BadRequest();
        }
        
        user = await _shootersDbContext.Users.FindAsync(id);
        
        if (user is null)
        {
            return NotFound();
        }
        
        user.Name = userDto.Name;
        user.Password = _authService.HashPassword(userDto.Password);
        userDto.UserRoleId = user.UserRoleId;
        
        try
        {
            _shootersDbContext.Entry(user).State = EntityState.Modified;
            await _shootersDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            
            return BadRequest(e.Message);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _shootersDbContext.Users.FindAsync(id);
        
        if (user is null)
        {
            return NotFound();
        }

        _shootersDbContext.Users.Remove(user);
        await _shootersDbContext.SaveChangesAsync();
        
        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _shootersDbContext.Users.Any(user => user.Id == id);
    }
}