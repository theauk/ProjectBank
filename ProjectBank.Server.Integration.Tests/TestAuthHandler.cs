using System;
using System.Collections.Generic;

namespace ProjectBank.Server.Integration.Tests;

internal sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IList<Claim> _claims;
    
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, TestClaimsProvider claimsProvider) : base(options, logger, encoder, clock)
    {
        _claims = claimsProvider.Claims;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {

        var identity = new ClaimsIdentity(_claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}

public class TestClaimsProvider
{
    public IList<Claim> Claims { get; }

    public TestClaimsProvider(IList<Claim> claims)
    {
        Claims = new List<Claim>();
    }

    public TestClaimsProvider()
    {
        Claims = new List<Claim>();
    }

    public static TestClaimsProvider WithSuperAdminClaims()
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "ProjectBank Employee"));
        provider.Claims.Add(new Claim(ClaimTypes.Role, "SuperAdmin"));
        provider.Claims.Add(new Claim("http://schemas.microsoft.com/identity/claims/scope", "API.Access"));

        return provider;
    }

    public static TestClaimsProvider WithAdminClaims()
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "Admin user"));
        provider.Claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        provider.Claims.Add(new Claim(ClaimTypes.Email, "marco@itu.dk"));
        provider.Claims.Add(new Claim("http://schemas.microsoft.com/identity/claims/scope", "API.Access"));

        return provider;
    }

    public static TestClaimsProvider WithSupervisorClaims()
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "Supervisor user"));
        provider.Claims.Add(new Claim(ClaimTypes.Role, "Supervisor"));
        provider.Claims.Add(new Claim("http://schemas.microsoft.com/identity/claims/scope", "API.Access"));

        return provider;
    }

    public static TestClaimsProvider WithStudentClaims()
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "Student user"));
        provider.Claims.Add(new Claim(ClaimTypes.Role, "Student"));
        provider.Claims.Add(new Claim("http://schemas.microsoft.com/identity/claims/scope", "API.Access"));

        return provider;
    }

    public static TestClaimsProvider WithUserClaims()
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "Annonymous User"));

        return provider;
    }
}
