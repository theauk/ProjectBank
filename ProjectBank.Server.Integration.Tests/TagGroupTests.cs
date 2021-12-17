using System.Collections.Generic;
using System.Linq;
using System.Net;
using Blazorise.Extensions;
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

        var response = await client.GetFromJsonAsync<IReadOnlyCollection<TagGroupDTO>>("api/TagGroup");

        Assert.NotNull(response);
        Assert.Collection(response,
            project => Assert.Equal(new TagGroupDTO() {  }, project)
        );
    }
    
    [Fact]
        public async Task updateTagGroup_with_id_1()
        {
            var provider = TestClaimsProvider.WithAdminClaims();
            var client = _factory.CreateClientWithTestAuth(provider);

            
            var updated = new TagGroupUpdateDTO()
            {
                Id = 1,
                Name = "Semester",
                RequiredInProject = true,
                SupervisorCanAddTag = false,
                TagLimit = 2, SelectedTagValues = {"Spring 2021", "Autumn 2022"}
            };
            var response = await client.PutAsJsonAsync("api/TagGroup", updated);
            Assert.Equal(response.StatusCode, HttpStatusCode.NoContent);
    
            var tgs = await client.GetFromJsonAsync<IReadOnlyCollection<TagGroupDTO>>("api/TagGroup");
            var newupdated = tgs.FirstOrDefault(t => t.Id.IsEqual(1));
            Assert.Equal(2, newupdated.TagDTOs.Count);
        }
}
