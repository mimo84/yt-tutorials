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

        var request = $@"{{
          ""userName"": ""{username}"",
          ""email"": ""{username}@email.com"",
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
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = JsonSerializer.Deserialize<UserDto>(stringResult, jsonSerializerOptions) ?? throw new Exception("Could not parse {stringResult}");
        result.Address.Should().Be("string");
        result.FamilyName.Should().Be("string");
        result.FirstName.Should().Be("string");
        result.Bio.Should().Be("string");
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async void ShouldNotRegisterExistingUser()
    {
        var request = $@"{{
          ""userName"": ""mimo@email.com"",
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

        Func<Task> act = async () => await _httpClient.PostAsync("/user/register", requestContent);
        var result = await act.Should().ThrowAsync<ProblemDetailsException>();
    }
}
