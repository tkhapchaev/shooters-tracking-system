using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShootersTrackingSystem.Model.Dto;
using ShootersTrackingSystem.Model.Services;

namespace ShootersTrackingSystem.Controllers;

[Authorize(Roles = "Admin,Instructor,Client")]
[AllowAnonymous]
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost]
    public async Task<ActionResult> Authenticate(AuthDto authDto)
    {
        try
        {
            var authResponseDto = await _authService.Authenticate(authDto.Username, authDto.Password);

            if (authResponseDto is null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(authResponseDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}