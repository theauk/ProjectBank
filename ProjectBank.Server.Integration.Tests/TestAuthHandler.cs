using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectBank.Server.Integration.Tests;

// Code taken from Rasmus Lystrøm

internal sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IList<Claim> _claims;
 
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, 
        UrlEncoder encoder, ISystemClock clock, TestClaimsProvider claimsProvider) : base(options, logger, encoder, clock) 
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
        Claims = claims;
    }
 
    public TestClaimsProvider()
    {
        Claims = new List<Claim>();
    }
 
    public static TestClaimsProvider WithAdminClaims()
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "Admin user"));
        provider.Claims.Add(new Claim(ClaimTypes.Role, "Admin"));
 
        return provider;
    }
 
    public static TestClaimsProvider WithUserClaims()
    {
        var provider = new TestClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "User"));
 
        return provider;
    }
}