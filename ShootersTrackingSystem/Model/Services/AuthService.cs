using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dtos;

namespace ShootersTrackingSystem.Model.Services;

public class AuthService
{
    private const int TokenExpirationHours = 24;
    private readonly IConfiguration _configuration;
    private readonly DatabaseRepository _databaseRepository;

    public AuthService(IConfiguration configuration, DatabaseRepository databaseRepository)
    {
        _configuration = configuration;
        _databaseRepository = databaseRepository;
    }
    
    public async Task<AuthResponseDto?> Authenticate(string username, string password)
    {
        var user = await _databaseRepository.Users.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Name == username);
        
        if (user is null)
        {
            return null;
        }

        if (!VerifyPassword(password, user.Password))
        {
            return null;
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Keys:JwtSecretKey"] ?? throw new ArgumentNullException("JwtSecretKey"));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.Name),
                new (ClaimTypes.Role, user.UserRole.Name)
            }),
            Expires = DateTime.UtcNow.AddHours(TokenExpirationHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var passwordHashed = HashPassword(user.Password);

        var authResponseDto = new AuthResponseDto
        {
            Username = user.Name, 
            Password = passwordHashed, 
            UserRole = user.UserRole.Name,
            Token = token.UnsafeToString(), 
            AuthDateTime = DateTime.UtcNow,
            TokenExpirationHours = DateTime.UtcNow.AddHours(TokenExpirationHours)
        };
        
        return authResponseDto;
    }

    public string HashPassword(string password)
    {
        var salt = GenerateSalt();
        
        var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return Convert.ToBase64String(salt) + "$" + hashedPassword;
    }
    
    public bool VerifyPassword(string enteredPassword, string storedPassword)
    {
        var parts = storedPassword.Split('$');
        
        if (parts.Length != 2)
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[0]);
        var storedHash = parts[1];
        
        var enteredHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: enteredPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        
        return enteredHash == storedHash;
    }
    
    [Obsolete]
    private byte[] GenerateSalt()
    {
        using var rng = new RNGCryptoServiceProvider();
        
        var salt = new byte[16];
        rng.GetBytes(salt);
            
        return salt;
    }
}