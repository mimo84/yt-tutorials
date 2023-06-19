using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using static Integration.ConfigureWebApplicationFactory;

namespace Integration.Endpoints.User;

public class LoginTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly ITestOutputHelper testOutputHelper;

    public LoginTest(CustomWebApplicationFactory<Program> _factory, ITestOutputHelper _testOutputHelper)
    {
        factory = _factory;
        testOutputHelper = _testOutputHelper;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async void ShouldLoginSeededUser()
    {
        var request = $@"{{
            ""email"": ""mimo@email.com"",
            ""password"": ""MyT3stPa$$w0rd""
        }}";
        HttpContent requestContent = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/user/login", requestContent);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async void ShouldAuthorizeWrongPasswordUser()
    {
        var request = $@"{{
            ""email"": ""mimo@email.com"",
            ""password"": ""I AM WRONG""
        }}";
        HttpContent requestContent = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/user/login", requestContent);
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async void ShouldAuthorizeWrongEmailUser()
    {
        var request = $@"{{
            ""email"": ""iamnotregistered@email.com"",
            ""password"": ""MyT3stPa$$w0rd""
        }}";
        HttpContent requestContent = new StringContent(request, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/user/login", requestContent);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
