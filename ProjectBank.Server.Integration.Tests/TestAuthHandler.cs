using System;
using System.Threading.Tasks;

namespace ProjectBank.Server.Integration.Tests;

// Code taken from Rasmus Lystrøm

internal sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Admin"),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
        };

        var identity = new ClaimsIdentity(claims, "Admin");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Admin");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}