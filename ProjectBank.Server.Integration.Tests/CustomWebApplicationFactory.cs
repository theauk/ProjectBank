using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.TestHost;
using ProjectBank.Infrastructure;
using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Server.Integration.Tests;

// Code taken from Rasmus Lystrøm
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ProjectBankContext>));

            if (dbContext != null)
            {
                services.Remove(dbContext);
            }

            /* Overriding policies and adding Test Scheme defined in TestAuthHandler */
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Test")
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            var connection = new SqliteConnection("Filename=:memory:");

            services.AddDbContext<ProjectBankContext>(options => options.UseSqlite(connection));

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ProjectBankContext>();
            appContext.Database.OpenConnection();
            appContext.Database.EnsureCreated();

            Seed(appContext);
        });

        builder.UseEnvironment("Integration");

        return base.CreateHost(builder);
    }

    private void Seed(ProjectBankContext context)
    {
        // --- Test data ---
        // Supervisors
        var marco = new User { Id = 1, Name = "Marco", Email = "marco@itu.dk" };
        var birgit = new User { Id = 2, Name = "Birgit", Email = "birgit@itu.dk" };
        var bjorn = new User { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk" };
        var paolo = new User { Id = 4, Name = "Paolo", Email = "paolo@itu.dk" };
        var rasmus = new User { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk" };

        // TagGroups
        var semesterTG = new TagGroup
        {
            Id = 1,
            Name = "Semester",
            RequiredInProject = true,
            SupervisorCanAddTag = false,
            TagLimit = 2,
        };

        var programmingLanguageTG = new TagGroup
        {
            Id = 2,
            Name = "Programming Language",
            RequiredInProject = false,
            SupervisorCanAddTag = true,
            TagLimit = 10,
        };

        var mandatoryProjectsTG = new TagGroup
        {
            Id = 3,
            Name = "Mandatory Project",
            RequiredInProject = false,
            SupervisorCanAddTag = false,
            TagLimit = 1,
        };

        var topicTG = new TagGroup
        {
            Id = 4,
            Name = "Topic",
            RequiredInProject = false,
            SupervisorCanAddTag = true,
            TagLimit = 10,
        };

        // Tags
        var math = new Tag { Id = 1, Value = "Math Theory", TagGroup = topicTG };
        var sql = new Tag { Id = 2, Value = "SQL", TagGroup = programmingLanguageTG };
        var goLang = new Tag { Id = 3, Value = "GoLang", TagGroup = programmingLanguageTG };
        var secondYear = new Tag { Id = 4, Value = "2nd Year Project", TagGroup = mandatoryProjectsTG };
        var spring22 = new Tag { Id = 5, Value = "Spring 2022", TagGroup = semesterTG };

        // Projects
        var mathProject = new Project { Id = 1, Name = "Math Project", Description = "Prove a lot of stuff.", Tags = new HashSet<Tag>() { math }, Supervisors = new HashSet<User>() { birgit } };
        var databaseProject = new Project { Id = 2, Name = "Database Project", Description = "Host a database with Docker.", Tags = new HashSet<Tag>() { sql, spring22 }, Supervisors = new HashSet<User>() { bjorn } };
        var goProject = new Project { Id = 3, Name = "Go Project", Description = "Create gRPC methods and connect it to SERF.", Tags = new HashSet<Tag>() { goLang }, Supervisors = new HashSet<User>() { marco } };
        var secondYearProject = new Project { Id = 4, Name = "Second Year Project", Description = "Group project in larger groups with a company.", Tags = new HashSet<Tag>() { secondYear, spring22 }, Supervisors = new HashSet<User>() { paolo, rasmus } };
        // -------------------

        // Universities
        var ituUni = new University
        {
            DomainName = "itu.dk",
            Projects = new HashSet<Project>() { mathProject, databaseProject, goProject, secondYearProject },
            TagGroups = new HashSet<TagGroup>() { semesterTG, programmingLanguageTG, mandatoryProjectsTG, topicTG },
            Users = new HashSet<User>() { marco, birgit, bjorn, paolo, rasmus }
        };

        context.Universities.Add(ituUni);

        context.SaveChanges();
    }
}

public static class WebApplicationFactoryExtensions
{
    public static WebApplicationFactory<T> WithAuthentication<T>(this WebApplicationFactory<T> factory, TestClaimsProvider claimsProvider) where T : class
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", op => { });
                services.AddScoped<TestClaimsProvider>(_ => claimsProvider);
            });
        });
    }

    public static HttpClient CreateClientWithTestAuth<T>(this WebApplicationFactory<T> factory, TestClaimsProvider claimsProvider) where T : class
    {
        var client = factory.WithAuthentication(claimsProvider).CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        return client;
    }
}
