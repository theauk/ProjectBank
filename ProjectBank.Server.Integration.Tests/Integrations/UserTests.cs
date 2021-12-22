using System.Collections.Generic;
using ProjectBank.Core;
using ProjectBank.Core.DTOs;

namespace ProjectBank.Server.Integration.Tests;

public class UserTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    
    private UserDTO marco = new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk" , Role = Role.Supervisor};
    private UserDTO birgit = new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Supervisor };
    private UserDTO bjorn = new UserDTO { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk", Role = Role.Supervisor };
    private UserDTO paolo = new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk", Role = Role.Supervisor };
    private UserDTO rasmus = new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Supervisor };
    private UserDTO dummy1 = new UserDTO { Id = 6, Name = "dummy1", Email = "dummy1@itu.dk", Role = Role.Supervisor }; //Dummies are for testing GetActive method/integration in UserController
    private UserDTO dummy2 = new UserDTO { Id = 7, Name = "dummy2", Email = "dummy2@itu.dk", Role = Role.Student };
    private UserDTO dummy3 = new UserDTO { Id = 8, Name = "dummy3", Email = "dummy3@itu.dk", Role = Role.Admin };
    
    public UserTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetActive_returns_Supervisors_assigned_to_project()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);

        var activeSupervisorsList = new List<UserDTO>()
        {
            marco,
            birgit,
            bjorn,
            paolo,
            rasmus,
        };
        
        //Act
        var actual = await client.GetFromJsonAsync<IReadOnlyCollection<UserDTO>>("api/User/filter");
        
        //Assert
        Assert.Equal(activeSupervisorsList, actual);
    }
    
    [Fact]
    public async Task Get_Role_Supervisor_returns_All_Supervisors()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        var rolesToGet = "roles=Supervisor";

        var SupervisorsList = new List<UserDTO>()
        {
            marco,
            birgit,
            bjorn,
            paolo,
            rasmus,
            dummy1
        };
        
        //Act
        var actual = await client.GetFromJsonAsync<IReadOnlyCollection<UserDTO>>("api/User/roles?" + rolesToGet);
        
        //Assert
        Assert.Equal(SupervisorsList, actual);
    }
}