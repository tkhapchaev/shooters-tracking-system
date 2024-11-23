using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using ShootersTrackingSystem.Model.Dto;
using Xunit;

namespace ShootersTrackingSystem.Tests;

public class ResultsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ResultsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetPersonalResults_ShouldReturnPersonalResults()
    {
        var client = _factory.CreateClient();
        var clientToken = await AuthorizeAsClient();
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken);
        var response = await client.GetAsync("/api/results/my");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetOtherUsersResult_ShouldNotReturnOtherUsersResult()
    {
        var client = _factory.CreateClient();
        var clientToken = await AuthorizeAsClient();
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken);
        var response = await client.GetAsync($"/api/results/by/user/1");
        
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetTopByWeapon_ShouldReturnTopByWeapon()
    {
        var client = _factory.CreateClient();
        var clientToken = await AuthorizeAsClient();
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken);
        var response = await client.GetAsync($"/api/results/by/weapon/1");
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var resultDtos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultDto>>(responseBody);

        if (resultDtos == null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(290, (int)resultDtos[0].Score);
        Assert.Equal(280, (int)resultDtos[1].Score);
        Assert.Equal(270, (int)resultDtos[2].Score);
        
        response = await client.GetAsync($"/api/results/by/weapon/2");
        responseBody = await response.Content.ReadAsStringAsync(); 
        resultDtos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultDto>>(responseBody);

        if (resultDtos == null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(300, (int)resultDtos[0].Score);
        Assert.Equal(240, (int)resultDtos[1].Score);
        Assert.Equal(170, (int)resultDtos[2].Score);

        response = await client.GetAsync($"/api/results/by/weapon/3");
        responseBody = await response.Content.ReadAsStringAsync();
        resultDtos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultDto>>(responseBody);

        if (resultDtos == null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(300, (int)resultDtos[0].Score);
        Assert.Equal(270, (int)resultDtos[1].Score);
        Assert.Equal(240, (int)resultDtos[2].Score);
    }

    [Fact]
    public async Task GetTopByWeaponType_ShouldReturnTopByWeaponType()
    {
        var client = _factory.CreateClient();
        var clientToken = await AuthorizeAsClient();
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken);
        var response = await client.GetAsync($"/api/results/by/weapon/1");
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var resultDtos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultDto>>(responseBody);

        if (resultDtos == null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(290, (int)resultDtos[0].Score);
        Assert.Equal(280, (int)resultDtos[1].Score);
        Assert.Equal(270, (int)resultDtos[2].Score);
        
        response = await client.GetAsync($"/api/results/by/weapon/2");
        responseBody = await response.Content.ReadAsStringAsync(); 
        resultDtos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultDto>>(responseBody);

        if (resultDtos == null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(300, (int)resultDtos[0].Score);
        Assert.Equal(240, (int)resultDtos[1].Score);
        Assert.Equal(170, (int)resultDtos[2].Score);

        response = await client.GetAsync($"/api/results/by/weapon/3");
        responseBody = await response.Content.ReadAsStringAsync();
        resultDtos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultDto>>(responseBody);

        if (resultDtos == null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(300, (int)resultDtos[0].Score);
        Assert.Equal(270, (int)resultDtos[1].Score);
        Assert.Equal(240, (int)resultDtos[2].Score);
    }

    [Fact]
    public async Task GetResultsByUserAndWeapon_ShouldReturnResultsByUserAndWeapon()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.GetAsync($"/api/results/by/userandweapon/3/1");
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var resultDto = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultDto>(responseBody);

        if (resultDto == null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(290, (int)resultDto.Score);
    }

    [Fact]
    public async Task GetResultsByUserAndWeaponType_ShouldReturnResultsByUserAndWeaponType()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.GetAsync($"/api/results/by/userandweapon/4/2");
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var resultDto = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultDto>(responseBody);

        if (resultDto == null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(240, (int)resultDto.Score);
    }

    [Fact]
    public async Task GetResultsByUser_ShouldReturnResultsByUser()
    {
        var client = _factory.CreateClient();
        var adminToken = await AuthorizeAsAdmin();
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);
        var response = await client.GetAsync($"/api/results/by/user/5/");
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var resultDtos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultDto>>(responseBody);

        if (resultDtos == null)
        {
            throw new NullReferenceException();
        }
        
        Assert.Equal(300, (int)resultDtos[0].Score);
        Assert.Equal(270, (int)resultDtos[1].Score);
        Assert.Equal(270, (int)resultDtos[2].Score);
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