using System.Collections.Generic;
using ProjectBank.Core.DTOs;

namespace ProjectBank.Server.Integration.Tests;

public class TagGroupTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public TagGroupTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_returns_TagGroups()
    {
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);

        var response = await client.GetFromJsonAsync<IReadOnlyCollection<ProjectDTO>>("api/TagGroup");

        Assert.NotNull(response);
        Assert.Collection(response,
            project => Assert.Equal(new ProjectDTO {  }, project)
        );
    }
}
