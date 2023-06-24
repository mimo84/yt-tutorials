using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using FluentAssertions.Specialized;
using FoodDiary.Core.Dto;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using static Integration.ConfigureWebApplicationFactory;

namespace Integration.Endpoints.User;

public class RegisterTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly ITestOutputHelper testOutputHelper;

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };


    private static Random random = new Random();

    public RegisterTest(CustomWebApplicationFactory<Program> _factory, ITestOutputHelper _testOutputHelper)
    {
        factory = _factory;
        testOutputHelper = _testOutputHelper;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async void ShouldRegisterNewUser()
    {
        // For simplicity we generate a random username and email
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var username = new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        var address = new string(Enumerable.Repeat(chars, 82)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        var firstName = new string(Enumerable.Repeat(chars, 15)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        var familyName = new string(Enumerable.Repeat(chars, 19)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        var bio = new string(Enumerable.Repeat(chars, 500)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        var displayName = $"{firstName}{familyName}";
        var request = $@"{{
          ""userName"": ""{username}"",
          ""email"": ""{username}@email.com"",
          ""password"": ""123ABCabc%^&"",
          ""displayName"": ""{displayName}"",
          ""bio"": ""{bio}"",
          ""firstName"": ""{firstName}"",
          ""familyName"": ""{familyName}"",
          ""address"": ""{address}""
        }}";
        HttpContent requestContent = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/user/register", requestContent);
        var stringResult = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = JsonSerializer.Deserialize<UserDto>(stringResult, jsonSerializerOptions) ?? throw new Exception("Could not parse {stringResult}");
        result.Address.Should().Be(address);
        result.FamilyName.Should().Be(familyName);
        result.FirstName.Should().Be(firstName);
        result.Bio.Should().Be(bio);
        result.DisplayName.Should().Be(displayName);
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async void ShouldNotRegisterExistingUser()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var username = new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        var request = $@"{{
          ""userName"": ""{username}"",
          ""email"": ""mimo@email.com"",
          ""password"": ""123ABCabc%^&"",
          ""displayName"": ""string"",
          ""bio"": ""string"",
          ""firstName"": ""string"",
          ""familyName"": ""string"",
          ""address"": ""string""
        }}";
        HttpContent requestContent = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/user/register", requestContent);
        var stringResult = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var expectedValidation = JsonSerializer.Deserialize<HttpValidationProblemDetails>(stringResult, jsonSerializerOptions) ?? throw new Exception($"Could not parse {stringResult}");
        expectedValidation.Errors.Count.Should().Be(1);
        expectedValidation.Errors.First().Key.Should().Be("email");
        expectedValidation.Errors.First().Value.Should().AllBe("Email taken");
    }

    [Fact]
    public async void ShouldNotRegisterExistingUserName()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var email = new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        var request = $@"{{
          ""userName"": ""mimo"",
          ""email"": ""{email}@email.com"",
          ""password"": ""123ABCabc%^&"",
          ""displayName"": ""string"",
          ""bio"": ""string"",
          ""firstName"": ""string"",
          ""familyName"": ""string"",
          ""address"": ""string""
        }}";
        HttpContent requestContent = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/user/register", requestContent);
        var stringResult = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var expectedValidation = JsonSerializer.Deserialize<HttpValidationProblemDetails>(stringResult, jsonSerializerOptions) ?? throw new Exception($"Could not parse {stringResult}");
        expectedValidation.Errors.Count.Should().Be(1);
        expectedValidation.Errors.First().Key.Should().Be("username");
        expectedValidation.Errors.First().Value.Should().AllBe("Username taken");
    }

    [Fact]
    public async void ShouldNotRegisterEasyPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var username = new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        var request = $@"{{
          ""userName"": ""{username}"",
          ""email"": ""{username}@email.com"",
          ""password"": ""easy"",
          ""displayName"": ""string"",
          ""bio"": ""string"",
          ""firstName"": ""string"",
          ""familyName"": ""string"",
          ""address"": ""string""
        }}";
        HttpContent requestContent = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/user/register", requestContent);
        var stringResult = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var expectedValidation = JsonSerializer.Deserialize<HttpValidationProblemDetails>(stringResult, jsonSerializerOptions) ?? throw new Exception($"Could not parse {stringResult}");
        expectedValidation.Detail.Should().Be("Cannot register user: Passwords must be at least 6 characters.Passwords must have at least one digit ('0'-'9').Passwords must have at least one uppercase ('A'-'Z').");
    }
}
