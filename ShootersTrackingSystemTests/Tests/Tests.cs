using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ShootersTrackingSystemTests.Tests;

public class Tests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public Tests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public void GetUsers_ShouldReturnAllUsers()
    {
        var a = _factory.CreateClient().GetAsync("/api/users").Result;
        Assert.NotNull(a);
    }

    [Fact]
    public void AddUser_ShouldAddUser()
    {
        
    }

    [Fact]
    public void EditUser_ShouldEditUser()
    {
        
    }

    [Fact]
    public void RemoveUser_ShouldRemoveUser()
    {
        
    }

    [Fact]
    public void GetAttempts_ShouldReturnAllAttempts()
    {
        
    }

    [Fact]
    public void AddAttempt_ShouldAddAttempt()
    {
        
    }

    [Fact]
    public void EditAttempt_ShouldEditAttempt()
    {
        
    }

    [Fact]
    public void RemoveAttempt_ShouldRemoveAttempt()
    {
        
    }

    [Fact]
    public void GetWeapons_ShouldReturnAllWeapons()
    {
        
    }

    [Fact]
    public void AddWeapons_ShouldReturnAllWeapons()
    {
        
    }

    [Fact]
    public void EditWeapon_ShouldEditWeapon()
    {
        
    }

    [Fact]
    public void RemoveWeapon_ShouldRemoveWeapon()
    {
        
    }

    [Fact]
    public void Authorize_ShouldAuthorizeUser()
    {
        
    }

    [Fact]
    public void GetPersonalResults_ShouldReturnPersonalResults()
    {
        
    }

    [Fact]
    public void GetOtherUsersResult_ShouldNotReturnOtherUsersResult()
    {
        
    }

    [Fact]
    public void GetTopByWeapon_ShouldReturnTopByWeapon()
    {
        
    }

    [Fact]
    public void GetTopByWeaponType_ShouldReturnTopByWeaponType()
    {
        
    }

    [Fact]
    public void GetResultsByUserAndWeapon_ShouldReturnResultsByUserAndWeapon()
    {
        
    }

    [Fact]
    public void GetResultsByUserAndWeaponType_ShouldReturnResultsByUserAndWeaponType()
    {
        
    }

    [Fact]
    public void GetResultsByUser_ShouldReturnResultsByUser()
    {
        
    }
}