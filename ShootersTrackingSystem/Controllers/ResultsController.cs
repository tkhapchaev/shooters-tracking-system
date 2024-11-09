using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dtos;
using ShootersTrackingSystem.Model.Entities;
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

    [HttpGet("byWeapon/{weaponId}")]
    public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByWeapon(int weaponId)
    {
        var results = await _resultsService.GetResultsByWeapon(weaponId);

        if (!results.Any())
        {
            return NotFound();
        }
        
        return Ok(results);
    }
    
    [HttpGet("byWeaponType/{weaponTypeId}")]
    public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByWeaponType(int weaponTypeId)
    {
        var results = await _resultsService.GetResultsByWeaponType(weaponTypeId);

        if (!results.Any())
        {
            return NotFound();
        }
        
        return Ok(results);
    }

    [HttpGet("byUser/{userId}")]
    public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByUser(int userId)
    {
        var results = await _resultsService.GetResultsByUser(userId);

        if (!results.Any())
        {
            return NotFound();
        }
        
        return Ok(results);
    }

    [HttpGet("byUserAndWeapon/{userId}/{weaponId}")]
    public async Task<ActionResult<ResultDto>> GetResultsByUserAndWeapon(int userId, int weaponId)
    {
        var results = await _resultsService.GetResultsByUserAndWeapon(userId, weaponId);

        if (results.Score == 0)
        {
            return NotFound();
        }
        
        return Ok(results);
    }

    [HttpGet("byUserAndWeaponType/{userId}/{weaponTypeId}")]
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