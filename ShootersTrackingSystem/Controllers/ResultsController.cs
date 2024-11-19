using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShootersTrackingSystem.Model.Dto;
using ShootersTrackingSystem.Model.Services;

namespace ShootersTrackingSystem.Controllers;

[Route("api/results")]
[ApiController]
public class ResultsController : ControllerBase
{
    private readonly ResultsService _resultsService;

    public ResultsController(ResultsService resultsService)
    {
        _resultsService = resultsService;
    }
    
    [HttpGet("my")]
    [Authorize(Roles = "Admin,Instructor,Client")]
    public async Task<ActionResult<ResultDto>> GetUserResults()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized("User id not found in token");
        }
        
        var results = await _resultsService.GetResultsByUser(int.Parse(userId));

        if (!results.Any())
        {
            return NotFound();
        }
        
        return Ok(results);
    }

    [HttpGet("by/weapon/{weaponId}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByWeapon(int weaponId)
    {
        var results = await _resultsService.GetResultsByWeapon(weaponId);

        if (!results.Any())
        {
            return NotFound();
        }
        
        return Ok(results);
    }
    
    [HttpGet("by/weapontype/{weaponTypeId}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByWeaponType(int weaponTypeId)
    {
        var results = await _resultsService.GetResultsByWeaponType(weaponTypeId);

        if (!results.Any())
        {
            return NotFound();
        }
        
        return Ok(results);
    }

    [HttpGet("by/user/{userId}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByUser(int userId)
    {
        var results = await _resultsService.GetResultsByUser(userId);

        if (!results.Any())
        {
            return NotFound();
        }
        
        return Ok(results);
    }

    [HttpGet("by/userandweapon/{userId}/{weaponId}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<ResultDto>> GetResultsByUserAndWeapon(int userId, int weaponId)
    {
        var results = await _resultsService.GetResultsByUserAndWeapon(userId, weaponId);

        if (results.Score == 0)
        {
            return NotFound();
        }
        
        return Ok(results);
    }

    [HttpGet("by/userandweapontype/{userId}/{weaponTypeId}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<ResultDto>> GetResultsByUserAndWeaponType(int userId, int weaponTypeId)
    {
        var results = await _resultsService.GetResultsByUserAndWeaponType(userId, weaponTypeId);

        if (results.Score == 0)
        {
            return NotFound();
        }
        
        return Ok(results);
    }
}