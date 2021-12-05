using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProjectBank.Infrastructure
{
    public static class SeedExtensions
    {
        public static async Task<IHost> SeedAsync(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProjectBankContext>();

                await SeedProjectBankAsync(context);
            }

            return host;
        }

        private static async Task SeedProjectBankAsync(ProjectBankContext context)
        {
            //await context.Database.MigrateAsync();
            
            // // Create universities
            // if (!await context.Universities.AnyAsync())
            // {

            // }

            // // Create users
            // if (!await context.Users.AnyAsync())
            // {

            // }

            // // Create Projects
            // if (!await context.Projects.AnyAsync())
            // {

            // }

            // // Create TagGroups
            // if (!await context.TagGroups.AnyAsync())
            // {

            // }

            // // Create Tags
            // if (!await context.Tags.AnyAsync())
            // {

            // }

            //await context.SaveChangesAsync();
        }
    }
}
