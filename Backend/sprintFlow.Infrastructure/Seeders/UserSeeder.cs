using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Constants;

namespace sprintFlow.Infrastructure.Repositories;

public class UserSeeder
{
    public static async Task SeedAsync(UserManager<User> userManager)
    {
        string adminEmail = "admin@sprintflow.com";
        string adminPassword = "Admin123!";

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