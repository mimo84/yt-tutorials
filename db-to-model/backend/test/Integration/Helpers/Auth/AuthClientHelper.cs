using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using static Integration.ConfigureWebApplicationFactory;

namespace Integration.Helpers.Auth;

public static class AuthClientHelper
{
    public static HttpClient GetAuthClient(CustomWebApplicationFactory<Program> _factory)
    {
        var client = _factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services
                        .AddAuthentication(defaultScheme: "TestScheme")
                        .AddScheme<AuthenticationSchemeOptions, TestRegularUserAuthHandler>(
                            "TestScheme",
                            options => { }
                        );
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            scheme: "TestScheme"
        );

        return client;
    }
}
