using System.IdentityModel.Tokens.Jwt;
using Backend.Helper;
using Backend.Models;
using FakeItEasy;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Xunit.Abstractions;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace BackendTest.Helper;

[Collection("JwtHelperTests")]
public class AuthHelperTest
{
    private readonly AuthHelper _authHelper;
    private readonly ITestOutputHelper _output;
    private readonly IConfiguration _configuration = A.Fake<IConfiguration>();
    private static User CreateFakeUser() => A.Fake<User>();
    private User _user = CreateFakeUser();

    public AuthHelperTest(ITestOutputHelper output)
    {
        _user.Email = "test@test.com";
        _user.Id = Guid.NewGuid();
        _authHelper = new AuthHelper(_configuration);
        _output = output;
    }


    [Fact]
    public void AuthHelper_GenerateToken_Valid()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "AppSettings:Token", "TestKey1234.#2POHJFSisdhfihOIHSDQOIHDQZAE0EI9ISQDKJSPQOJFSQDBJLQBSDFSQD" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        
        var jwtHelper = new AuthHelper(configuration);
        
        var jwt = jwtHelper.GenerateToken(_user);
        Assert.NotEmpty(jwt);
        Assert.NotNull(jwt);
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(jwt);
        Assert.NotNull(jwtToken);
        Assert.Equal(_user.Email, jwtToken.Claims.First(c => c.Type == "email").Value);
        Assert.Equal(_user.Id.ToString(), jwtToken.Claims.First(c => c.Type == "nameid").Value);
    }
    
    [Fact]
    public void AuthHelper_GenerateToken_Invalid()
    {
        const string jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRlc3RAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiNDdhMDBkYWUtZGY3ZC00OGI2LTk2YjEtZjdhNzY2MGQyM2JjIiwibmJmIjoxNzQxNTMyNDQzLCJl|sdpfohjsqopihfgomiqshdfg|eHAiOjE3NDIxMzcyNDMsImlhdCI6MTc0MTUzMjQ0M30.hFJYL9_EHTsnxH-RCN2Dht51AQjin96XSOhhYj99F3s";
        var handler = new JwtSecurityTokenHandler();
        Assert.Throws<SecurityTokenMalformedException>(() => handler.ReadJwtToken(jwt));
    }

    [Fact]
    public void AuthHelper_GenerateToken_WithoutKey()
    {
        Assert.Throws<InvalidOperationException>(() => _authHelper.GenerateToken(_user));
    }
}