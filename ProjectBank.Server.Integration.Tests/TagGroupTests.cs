

using System.Collections.Generic;
using System.Linq;
using ProjectBank.Core.DTOs;

namespace ProjectBank.Server.Integration.Tests;

public class TagGroupTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly TestClaimsProvider _provider;
    
    public TagGroupTests(CustomWebApplicationFactory factory)
    {
        _provider = TestClaimsProvider.WithAdminClaims();
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        //_client = factory.CreateClientWithTestAuth(_provider);
    }


    [Fact]
    public async Task Get_returns_projects()
    {
        var tagGroupDtos = await _client.GetFromJsonAsync<IEnumerable<ProjectDTO>>("api/Project");
        
        Assert.NotNull(tagGroupDtos);
        //Assert.True(tagGroupDtos.Count() == 2);
        int tagcount = 0;
        //foreach (var tagGroupDto in tagGroupDtos)
        //{
        //    tagcount += tagGroupDto.TagDTOs.Count;
        //}
       // Assert.Equal(9, tagcount);
        
    }
}