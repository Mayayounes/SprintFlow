using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace sprintFlow.Infrastructure.Repositories;

public class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await RoleSeeder.SeedAsync(roleManager);

    }
}
