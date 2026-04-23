using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Infrastructure.Seeders;

public class UserSeeder
{
    public static async Task SeedAsync(UserManager<User> userManager)
    {
        var adminEmail = "admin@sprintflow.com";
        var adminPassword = "Admin@12345";

        var existingUser = await userManager.FindByEmailAsync(adminEmail);

        if (existingUser != null)
            return;

        var adminUser = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);

        if (!result.Succeeded)
            throw new Exception("Failed to create admin user");

        await userManager.AddToRoleAsync(adminUser, UserRole.Admin.ToString());
    }
}
