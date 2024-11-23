using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using ShootersTrackingSystem.Model.Entities;
using Xunit;

namespace ShootersTrackingSystem.Tests;

public class WeaponsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeaponsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWeapons_ShouldReturnAllWeapons()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.GetAsync("/api/weapons");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddWeapon_ShouldAddWeapon()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        var requestBody = new
        {
            Name = Guid.NewGuid().ToString(),
            WeaponTypeId = 1
        };

        var jsonContent = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json"
        );
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.PostAsync("/api/weapons", jsonContent);
        
        var responseBody = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

        if (responseBody is null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        await client.DeleteAsync($"/api/weapons/{responseBody.Id}");
    }

    [Fact]
    public async Task EditWeapon_ShouldEditWeapon()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        var requestBody = new
        {
            Name = Guid.NewGuid().ToString(),
            WeaponTypeId = 1
        };

        var jsonContent = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json"
        );
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.PostAsync("/api/weapons", jsonContent);
        
        var responseBody = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

        if (responseBody is null)
        {
            throw new NullReferenceException();
        }
        
        requestBody = new
        {
            Name = Guid.NewGuid().ToString(),
            WeaponTypeId = 1
        };

        jsonContent = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json"
        );
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        response = await client.PutAsync($"/api/weapons/{responseBody.Id}", jsonContent);
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
        await client.DeleteAsync($"/api/weapons/{responseBody.Id}");
    }

    [Fact]
    public async Task RemoveWeapon_ShouldRemoveWeapon()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        var requestBody = new
        {
            Name = Guid.NewGuid().ToString(),
            WeaponTypeId = 1
        };

        var jsonContent = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json"
        );
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.PostAsync("/api/weapons", jsonContent);
        
        var responseBody = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

        if (responseBody is null)
        {
            throw new NullReferenceException();
        }
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        response = await client.DeleteAsync($"/api/weapons/{responseBody.Id}");
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private async Task<string> AuthorizeAsAdmin()
    {
        var client = _factory.CreateClient();

        var requestBody = new
        {
            username = "Admin",
            password = "Admin"
        };

        var jsonContent = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync("/api/auth", jsonContent);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var jsonResponseBody = JObject.Parse(responseBody);
        
        var token = jsonResponseBody["token"]?.ToString();

        if (token is null || response.StatusCode != HttpStatusCode.OK)
        {
            throw new NullReferenceException();
        }
        
        return token;
    }
}