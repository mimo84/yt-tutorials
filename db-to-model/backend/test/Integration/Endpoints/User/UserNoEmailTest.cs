using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using FluentAssertions;
using Integration.Helpers.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using static Integration.ConfigureWebApplicationFactory;

namespace Integration.Endpoints.User;

public class UserNoEmailTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient authHttpClient;
    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly ITestOutputHelper testOutputHelper;
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new() { PropertyNameCaseInsensitive = true };

    public UserNoEmailTest(
        CustomWebApplicationFactory<Program> _factory,
        ITestOutputHelper _testOutputHelper
    )
    {
        factory = _factory;
        testOutputHelper = _testOutputHelper;
        var client = _factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services
                        .AddAuthentication(defaultScheme: "TestScheme")
                        .AddScheme<AuthenticationSchemeOptions, TestNoEmailUserAuthHandler>(
                            "TestScheme",
                            options => { }
                        );
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            scheme: "TestScheme"
        );

        authHttpClient = client;
    }

    [Fact]
    public async Task CheckAuthAsync()
    {
        var response = await authHttpClient.GetAsync("/user");
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var stringResult = await response.Content.ReadAsStringAsync();
        var result =
            JsonSerializer.Deserialize<HttpValidationProblemDetails>(
                stringResult,
                jsonSerializerOptions
            ) ?? throw new Exception("Could not parse {stringResult}");
        result.Title.Should().Be("Internal Server Error");
    }
}
