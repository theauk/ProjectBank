using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.TestHost;
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
                options.DefaultScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            var connection = new SqliteConnection("Filename=:memory:");

            services.AddDbContext<ProjectBankContext>(options =>
            {
                options.UseSqlite(connection);
            });

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
        var numtheo = new Tag() {Id = 1, Value = "Number Theory"};
        var crypto = new Tag() {Id = 2, Value = "Cryptography"};
        var setTheo = new Tag() {Id = 3, Value = "Set Theory"};
        var regex = new Tag() {Id = 4, Value = "RegEx's"};
        var autom = new Tag() {Id = 5, Value = "Automatas"};

        var spring22 = new Tag() {Id = 12, Value = "Spring 2022"};
        var autumn22 = new Tag() {Id = 13, Value = "Autumn 2022"};
        var spring23 = new Tag() {Id = 14, Value = "Spring 2023"};
        var autumn23 = new Tag() {Id = 15, Value = "Autumn 2023"};

        var semester = new TagGroup()
        {
            Id = 31,
            Name = "Semester",
            Tags = new HashSet<Tag>() {spring22, spring23, autumn22, autumn23},
            SupervisorCanAddTag = false,
            RequiredInProject = false,
            TagLimit = 999,
        };
        
        var topic = new TagGroup()
        {
            Id = 32,
            Name = "Topic",
            Tags = new HashSet<Tag>() {numtheo, crypto,setTheo,regex,autom},
            SupervisorCanAddTag = true,
            RequiredInProject = false,
            TagLimit = 999,
        };

        context.TagGroups.AddRange(semester, topic);

        context.SaveChanges();
    }
}