using BankLoanApp3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class DbInitializer
{
    public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Seed roles
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        // Seed admin user
        if (userManager.FindByEmailAsync("admin@example.com").Result == null)
        {
            var user = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
            };

            var result = await userManager.CreateAsync(user, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        // Seed regular user
        if (userManager.FindByEmailAsync("user@example.com").Result == null)
        {
            var user = new ApplicationUser
            {
                UserName = "user@example.com",
                Email = "user@example.com",
            };

            var result = await userManager.CreateAsync(user, "User@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}


