using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using ShootersTrackingSystem.Model.Entities;
using Xunit;

namespace ShootersTrackingSystem.Tests;

public class UsersTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UsersTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task GetUsers_ShouldReturnAllUsers()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.GetAsync("/api/users");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddUser_ShouldAddUser()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        var requestBody = new
        {
            Name = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            UserRoleId = 3
        };

        var jsonContent = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json"
        );
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.PostAsync("/api/users", jsonContent);
        
        var responseBody = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

        if (responseBody is null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        await client.DeleteAsync($"/api/users/{responseBody.Id}");
    }

    [Fact]
    public async Task EditUser_ShouldEditUser()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        var requestBody = new
        {
            Name = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            UserRoleId = 3
        };

        var jsonContent = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json"
        );
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.PostAsync("/api/users", jsonContent);
        
        var responseBody = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

        if (responseBody is null)
        {
            throw new NullReferenceException();
        }
        
        requestBody = new
        {
            Name = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            UserRoleId = 3
        };

        jsonContent = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json"
        );
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        response = await client.PutAsync($"/api/users/{responseBody.Id}", jsonContent);
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
        await client.DeleteAsync($"/api/users/{responseBody.Id}");
    }

    [Fact]
    public async Task RemoveUser_ShouldRemoveUser()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        var requestBody = new
        {
            Name = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            UserRoleId = 3
        };

        var jsonContent = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json"
        );
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.PostAsync("/api/users", jsonContent);
        
        var responseBody = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

        if (responseBody is null)
        {
            throw new NullReferenceException();
        }
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        response = await client.DeleteAsync($"/api/users/{responseBody.Id}");
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task Authorize_ShouldAuthorizeUser()
    {
        var adminToken = await AuthorizeAsAdmin();
        var instructorToken = await AuthorizeAsInstructor();
        var clientToken = await AuthorizeAsClient();
        
        Assert.NotNull(adminToken);
        Assert.NotNull(instructorToken);
        Assert.NotNull(clientToken);
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
    
    private async Task<string> AuthorizeAsInstructor()
    {
        var client = _factory.CreateClient();

        var requestBody = new
        {
            username = "Instructor",
            password = "Instructor"
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
    
    private async Task<string> AuthorizeAsClient()
    {
        var client = _factory.CreateClient();

        var requestBody = new
        {
            username = "Client1",
            password = "Client"
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