namespace ShootersTrackingSystem.Model.Dtos;

public class AuthResponseDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string UserRole { get; set; }
    public string Token { get; set; }
    public DateTime AuthDateTime { get; set; }
    public DateTime TokenExpirationHours { get; set; }
}