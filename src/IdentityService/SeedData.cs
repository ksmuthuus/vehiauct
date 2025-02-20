using System.Security.Claims;
using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (userMgr.Users.Any()) return;

        var alice = userMgr.FindByNameAsync("muthu").Result;
        if (alice == null)
        {
            alice = new ApplicationUser
            {
                UserName = "muthu",
                Email = "muthuks@email.com",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(alice, "Pass123!").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(alice, [
                            new Claim(JwtClaimTypes.Name, "Muthu Venkatachalam"),
                        ]).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("muthu created");
        }
        else
        {
            Log.Debug("muthu already exists");
        }

        var bob = userMgr.FindByNameAsync("raj").Result;
        if (bob == null)
        {
            bob = new ApplicationUser
            {
                UserName = "raj",
                Email = "rajraja@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(bob, "Pass123!").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(bob, [
                            new Claim(JwtClaimTypes.Name, "Raj Raja"),
                        ]).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("raj created");
        }
        else
        {
            Log.Debug("raj already exists");
        }
    }
}
