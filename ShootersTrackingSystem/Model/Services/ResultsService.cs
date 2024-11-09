using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Dtos;

namespace ShootersTrackingSystem.Model.Services;

public class ResultsService
{
    private const int NumberOfBestAttemptsByUser = 3;
    private readonly DatabaseRepository _databaseRepository;

    public ResultsService(DatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }
    
    public async Task<IEnumerable<ResultDto>> GetResultsByWeapon(int weaponId)
    {
        var weapon = await _databaseRepository.Weapons.FindAsync(weaponId);

        if (weapon is null)
        {
            return new List<ResultDto>();
        }
        
        var users = await _databaseRepository.Users.Include(user => user.UserRole).ToListAsync();
        var results = new List<ResultDto>();
        
        foreach (var user in users)
        {
            var attempts = await _databaseRepository.Attempts
                .Where(attempt => attempt.UserId == user.Id)
                .Where(attempt => attempt.WeaponId == weaponId)
                .ToListAsync();

            if (attempts.Count == 0)
            {
                continue;
            }
            
            var attemptsOrdered = attempts.OrderByDescending(attempt => attempt.Score).Take(NumberOfBestAttemptsByUser);
            var total = attemptsOrdered.Sum(attempt => attempt.Score);
            
            results.Add(new ResultDto { Username = user.Name, Description = weapon.Name, Score = (ulong)total });
        }
        
        var resultsOrdered = results.OrderByDescending(result => result.Score);

        return resultsOrdered;
    }
    
    public async Task<IEnumerable<ResultDto>> GetResultsByWeaponType(int weaponTypeId)
    {
        var weaponType = await _databaseRepository.WeaponTypes.FindAsync(weaponTypeId);

        if (weaponType is null)
        {
            return new List<ResultDto>();
        }
        
        var users = await _databaseRepository.Users.Include(user => user.UserRole).ToListAsync();
        var results = new List<ResultDto>();
        
        foreach (var user in users)
        {
            var attempts = await _databaseRepository.Attempts
                .Where(attempt => attempt.UserId == user.Id)
                .Where(attempt => attempt.Weapon.WeaponTypeId == weaponTypeId)
                .ToListAsync();

            if (attempts.Count == 0)
            {
                continue;
            }
            
            var attemptsOrdered = attempts.OrderByDescending(attempt => attempt.Score).Take(NumberOfBestAttemptsByUser);
            var total = attemptsOrdered.Sum(attempt => attempt.Score);
            
            results.Add(new ResultDto { Username = user.Name, Description = weaponType.Name, Score = (ulong)total });
        }
        
        var resultsOrdered = results.OrderByDescending(result => result.Score);

        return resultsOrdered;
    }
    
    public async Task<IEnumerable<ResultDto>> GetResultsByUser(int userId)
    {
        var user = await _databaseRepository.Users.FindAsync(userId);

        if (user is null)
        {
            return new List<ResultDto>();
        }
        
        var weapons = await _databaseRepository.Weapons.Include(weapon => weapon.WeaponType).ToListAsync();
        var results = new List<ResultDto>();
        
        foreach (var weapon in weapons)
        {
            var attempts = await _databaseRepository.Attempts
                .Where(attempt => attempt.UserId == userId)
                .Where(attempt => attempt.WeaponId == weapon.Id)
                .ToListAsync();

            if (attempts.Count == 0)
            {
                continue;
            }
            
            var attemptsOrdered = attempts.OrderByDescending(attempt => attempt.Score).Take(NumberOfBestAttemptsByUser);
            var total = attemptsOrdered.Sum(attempt => attempt.Score);
            
            results.Add(new ResultDto { Username = user.Name, Description = weapon.Name, Score = (ulong)total });
        }
        
        var resultsOrdered = results.OrderByDescending(result => result.Score);

        return resultsOrdered;
    }
    
    public async Task<ResultDto> GetResultsByUserAndWeapon(int userId, int weaponId)
    {
        var user = await _databaseRepository.Users.FindAsync(userId);

        if (user is null)
        {
            return new ResultDto();
        }
        
        var weapon = await _databaseRepository.Weapons.FindAsync(weaponId);

        if (weapon is null)
        {
            return new ResultDto();
        }
        
        var attempts = await _databaseRepository.Attempts
            .Where(attempt => attempt.UserId == userId)
            .Where(attempt => attempt.WeaponId == weaponId)
            .ToListAsync();

        if (attempts.Count == 0)
        {
            return new ResultDto();
        }
            
        var attemptsOrdered = attempts.OrderByDescending(attempt => attempt.Score).Take(NumberOfBestAttemptsByUser);
        var total = attemptsOrdered.Sum(attempt => attempt.Score);
            
        return new ResultDto { Username = user.Name, Description = weapon.Name, Score = (ulong)total };
    }
    
    public async Task<ResultDto> GetResultsByUserAndWeaponType(int userId, int weaponTypeId)
    {
        var user = await _databaseRepository.Users.FindAsync(userId);

        if (user is null)
        {
            return new ResultDto();
        }
        
        var weaponType = await _databaseRepository.WeaponTypes.FindAsync(weaponTypeId);

        if (weaponType is null)
        {
            return new ResultDto();
        }
        
        var attempts = await _databaseRepository.Attempts
            .Where(attempt => attempt.UserId == userId)
            .Where(attempt => attempt.Weapon.WeaponTypeId == weaponTypeId)
            .ToListAsync();

        if (attempts.Count == 0)
        {
            return new ResultDto();
        }
            
        var attemptsOrdered = attempts.OrderByDescending(attempt => attempt.Score).Take(NumberOfBestAttemptsByUser);
        var total = attemptsOrdered.Sum(attempt => attempt.Score);
            
        return new ResultDto { Username = user.Name, Description = weaponType.Name, Score = (ulong)total };
    }
}