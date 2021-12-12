namespace ProjectBank.Server.Integration.Tests;

public class TagGroupTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public TagGroupTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    // [Fact]
    // public async Task Get_returns_TagGroups()
    // {
    //     var provider = TestClaimsProvider.WithAdminClaims();
    //     var client = _factory.CreateClientWithTestAuth(provider);
    //     //var response = await client.GetFromJsonAsync<IReadOnlyCollection<TagGroupDTO>>("api/Project");
    //     var _response = await client.GetAsync("api/Project");


    //     // var tagGroups = await _client.GetFromJsonAsync<TagGroupDTO[]>("/api/TagGroup");
    //     // Assert.NotNull(tagGroups);
    //     // Assert.True(tagGroups.Length >= 4);
    //     // Assert.Contains(tagGroups, tg => tg.Name == "Semester");
    // }


    // private readonly HttpClient _client;
    // private readonly CustomWebApplicationFactory _factory;
    // private readonly TestClaimsProvider _provider;
    
    // public TagGroupTests(CustomWebApplicationFactory factory)
    // {
    //     _provider = TestClaimsProvider.WithAdminClaims();
    //     _factory = factory;
    //     _client = factory.CreateClient(new WebApplicationFactoryClientOptions
    //     {
    //         AllowAutoRedirect = false
    //     });
    //     //_client = factory.CreateClientWithTestAuth(_provider);
    // }


    // [Fact]
    // public async Task Get_returns_projects()
    // {
    //     var tagGroupDtos = await _client.GetFromJsonAsync<IEnumerable<ProjectDTO>>("api/Project");
        
    //     Assert.NotNull(tagGroupDtos);
    //     //Assert.True(tagGroupDtos.Count() == 2);
    //     int tagcount = 0;
    //     //foreach (var tagGroupDto in tagGroupDtos)
    //     //{
    //     //    tagcount += tagGroupDto.TagDTOs.Count;
    //     //}
    //    // Assert.Equal(9, tagcount);
        
    // }
}