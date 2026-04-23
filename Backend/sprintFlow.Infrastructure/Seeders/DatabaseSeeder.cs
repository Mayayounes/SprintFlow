using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using sprintFlow.Domain.Entities;
using sprintFlow.Infrastructure.Seeders;

namespace sprintFlow.Infrastructure.Repositories;

public class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        await RoleSeeder.SeedAsync(roleManager);
        await UserSeeder.SeedAsync(userManager);

    }
}
