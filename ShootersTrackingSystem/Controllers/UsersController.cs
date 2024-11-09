using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dtos;
using ShootersTrackingSystem.Model.Entities;

namespace ShootersTrackingSystem.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DatabaseRepository _databaseRepository;

    public UsersController(DatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _databaseRepository.Users.Include(user => user.UserRole).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _databaseRepository.Users.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Id == id);
        
        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(UserDto userDto)
    {
        var user = await _databaseRepository.Users.FirstOrDefaultAsync(u => u.Name == userDto.Name);

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

        try
        {
            _databaseRepository.Users.Add(user);
            await _databaseRepository.SaveChangesAsync();
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
        var user = await _databaseRepository.Users.FirstOrDefaultAsync(u => u.Name == userDto.Name);

        if (user is not null)
        {
            return BadRequest();
        }
        
        user = await _databaseRepository.Users.FindAsync(id);
        
        if (user is null)
        {
            return NotFound();
        }
        
        user.Name = userDto.Name;
        user.Password = userDto.Password;
        userDto.UserRoleId = user.UserRoleId;
        
        try
        {
            _databaseRepository.Entry(user).State = EntityState.Modified;
            await _databaseRepository.SaveChangesAsync();
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
        var user = await _databaseRepository.Users.FindAsync(id);
        
        if (user is null)
        {
            return NotFound();
        }

        _databaseRepository.Users.Remove(user);
        await _databaseRepository.SaveChangesAsync();
        
        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _databaseRepository.Users.Any(user => user.Id == id);
    }
}