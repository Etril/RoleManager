using System.Data.Common;
using Backend.Domain.Entities;
using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using Backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DataBaseSeeding
{
    public static async Task SeedAsync (this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        await db.Database.MigrateAsync();

        if (await db.Users.AnyAsync())
        return; 

        var baseUser= new User (new UserName("base"), PasswordHash.FromPlainText("base"), RoleRights.CreateBaseUser());
        var adminUser= new User (new UserName("admin"), PasswordHash.FromPlainText("admin"), RoleRights.CreateAdmin());
        
        var testOrder1= new Order (new OrderName("Laptop"), 100, new OrderDate(DateTime.UtcNow));
        var testOrder2= new Order (new OrderName("Chair"), 59, new OrderDate(DateTime.UtcNow));
        var testOrder3= new Order (new OrderName("Desk"), 500, new OrderDate(DateTime.UtcNow));


        adminUser.AddOrder(testOrder1, baseUser);
        adminUser.AddOrder(testOrder2, baseUser);
        adminUser.AddOrder(testOrder3, baseUser);

        db.Users.Add(adminUser);
        db.Users.Add(baseUser);

        await db.SaveChangesAsync();



    }
}