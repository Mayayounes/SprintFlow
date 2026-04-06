using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Infrastructure.Repositories;

public class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = Enum.GetNames(typeof(UserRole));

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded)
                    throw new Exception($"Failed to create role: {role}");
            }
        }
    }
}
