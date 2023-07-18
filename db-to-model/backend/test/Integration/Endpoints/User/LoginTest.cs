using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using FoodDiary.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using static Integration.ConfigureWebApplicationFactory;

namespace Integration.Endpoints.User;

public class LoginTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly ITestOutputHelper testOutputHelper;

    private readonly JsonSerializerOptions jsonSerializerOptions =
        new() { PropertyNameCaseInsensitive = true };

    public LoginTest(
        CustomWebApplicationFactory<Program> _factory,
        ITestOutputHelper _testOutputHelper
    )
    {
        factory = _factory;
        testOutputHelper = _testOutputHelper;
        _httpClient = factory.CreateClient(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = false }
        );
    }

    [Fact]
    public async void ShouldLoginSeededUser()
    {
        var request =
            $@"{{
            ""email"": ""mimo@email.com"",
            ""password"": ""MyT3stPa$$w0rd""
        }}";
        HttpContent requestContent = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/user/login", requestContent);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var stringResult = await response.Content.ReadAsStringAsync();
        var result =
            JsonSerializer.Deserialize<UserDto>(stringResult, jsonSerializerOptions)
            ?? throw new Exception("Could not parse {stringResult}");
        result.DisplayName.Should().Be("Mimo");
        result.Token.Should().NotBeNullOrEmpty();

        var token = result.Token;
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token);
        var tokenS =
            jsonToken as JwtSecurityToken ?? throw new XunitException("Token could not be found");
        tokenS.Claims.First(claim => claim.Type == "unique_name").Value.Should().Be("mimo");
        tokenS.Claims.First(claim => claim.Type == "nameid").Value.Should().NotBeNullOrWhiteSpace();
        tokenS.Claims.First(claim => claim.Type == "email").Value.Should().Be("mimo@email.com");
        tokenS.Claims.First(claim => claim.Type == "nbf").Value.Should().NotBeNullOrEmpty();
        tokenS.Claims.First(claim => claim.Type == "exp").Value.Should().NotBeNullOrEmpty();
        tokenS.Claims.First(claim => claim.Type == "iat").Value.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async void ShouldNotAuthorizeWrongPasswordUser()
    {
        var request =
            $@"{{
            ""email"": ""mimo@email.com"",
            ""password"": ""I AM WRONG""
        }}";
        HttpContent requestContent = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/user/login", requestContent);
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var stringResult = await response.Content.ReadAsStringAsync();
        var result =
            JsonSerializer.Deserialize<HttpValidationProblemDetails>(
                stringResult,
                jsonSerializerOptions
            ) ?? throw new Exception("Could not parse {stringResult}");
        result.Title.Should().Be("One or more validation errors occurred.");
        result.Detail.Should().Be("Incorrect Credentials");
    }

    [Fact]
    public async void ShouldNotAuthorizeWrongEmailUser()
    {
        var request =
            $@"{{
            ""email"": ""iamnotregistered@email.com"",
            ""password"": ""MyT3stPa$$w0rd""
        }}";
        HttpContent requestContent = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/user/login", requestContent);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var stringResult = await response.Content.ReadAsStringAsync();
        var result =
            JsonSerializer.Deserialize<HttpValidationProblemDetails>(
                stringResult,
                jsonSerializerOptions
            ) ?? throw new Exception("Could not parse {stringResult}");
        result.Title.Should().Be("Unauthorized");
    }

    [Fact]
    public async void ShouldNotAuthorizeUserEndpointWithNoAuth()
    {
        var response = await _httpClient.GetAsync("/user");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
