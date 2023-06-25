using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Integration.Helpers.Auth;
public class TestRegularUserAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestRegularUserAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] {
          new Claim(ClaimTypes.Name, "Test user"),
          new Claim(ClaimTypes.NameIdentifier, "6f1491fa-499d-4e5b-aa1c-ec365eb4303b"),
          new Claim(ClaimTypes.Email, "mimo@email.com")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}