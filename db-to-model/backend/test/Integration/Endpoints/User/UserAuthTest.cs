using System.Net;
using System.Text.Json;
using FluentAssertions;
using FoodDiary.Core.Dto;
using Integration.Helpers.Auth;
using Xunit;
using Xunit.Abstractions;
using static Integration.ConfigureWebApplicationFactory;

namespace Integration.Endpoints.User;

public class UserTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient authHttpClient;
    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly ITestOutputHelper testOutputHelper;
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new() { PropertyNameCaseInsensitive = true };

    public UserTest(
        CustomWebApplicationFactory<Program> _factory,
        ITestOutputHelper _testOutputHelper
    )
    {
        factory = _factory;
        testOutputHelper = _testOutputHelper;
        authHttpClient = AuthClientHelper.GetAuthClient(_factory);
    }

    [Fact]
    public async Task CheckAuthAsync()
    {
        var response = await authHttpClient.GetAsync("/user");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var stringResult = await response.Content.ReadAsStringAsync();
        var result =
            JsonSerializer.Deserialize<UserDto>(stringResult, jsonSerializerOptions)
            ?? throw new Exception("Could not parse {stringResult}");
        result.DisplayName.Should().NotBeNullOrEmpty();
        result.Token.Should().NotBeNullOrEmpty();
    }
}