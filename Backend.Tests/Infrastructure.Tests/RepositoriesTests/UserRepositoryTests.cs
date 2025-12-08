using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;
using Backend.Domain.Roles;
using FluentAssertions;
using Microsoft.VisualBasic;
using Infrastructure.Tests.Common;
using Backend.Infrastructure.Persistence.Repositories;
using SQLitePCL;

namespace Infrastructure.Tests.RepositoriesTests; 

public class UserRepositoryTests
{
    [Fact]
    public async Task GetByIdAsync_ReturnsUser_WhenValid()
    {
        //Arrange
        using var context=TestDbContextFactory.Create();
        var repo=new UserRepository(context);

        var user= new User(new UserName("Test"), PasswordHash.FromHash("hashedpassword"), RoleRights.CreateBaseUser());

        context.Users.Add(user);
        await context.SaveChangesAsync();

        //Act
        var result= await repo.GetByIdAsync(user.Id);

        //Assert
        result.Should().Be(user);
        result.Username.Value.Should().Be("Test");

    }

    [Fact]

    public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        //Arrange
        using var context= TestDbContextFactory.Create();
        var repo= new UserRepository(context);
        var fakeUserId= Guid.NewGuid();

        //Act
        var result= await repo.GetByIdAsync(fakeUserId);

        //Assert
        result.Should().Be(null);
    }

    [Fact]

    public async Task GetByOrderIdAsync_ReturnsUser_WhenValid()
    {
        //Arrange
        using var context = TestDbContextFactory.Create();
        var repo= new UserRepository(context);

        var user = new User (new UserName("Test123"), PasswordHash.FromHash("hashedpassword"), RoleRights.CreateBaseUser());
        var order= new Order (new OrderName("ordertest"), 100, new OrderDate(DateTime.UtcNow));
        user.AddOrder(order, user);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        //Act
        var result= await repo.GetByOrderIdAsync(order.Id);

        //Assert
        result.Should().Be(user);
        result.Orders.Should().ContainSingle(o => o.Id == order.Id);

    }

    [Fact]

    public async Task UpdateAsync_AddsNewOrder_Properly()
    {
        //Arrange
        using var context= TestDbContextFactory.Create();
        var repo= new UserRepository(context);

        var user = new User (new UserName("test123"), PasswordHash.FromHash("hashedpassword"), RoleRights.CreateBaseUser());
        var order= new Order(new OrderName("ordertest"), 100, new OrderDate(DateTime.UtcNow));

        user.AddOrder(order, user);
        context.Users.Add(user);
        user.UpdateOrderPrice(order.Id, 1000, user);
        await context.SaveChangesAsync();
        

        //Act
        
        await repo.UpdateAsync(user);

        //Assert
        var dbUser = await repo.GetByIdAsync(user.Id);
        var updatedOrder = dbUser?.Orders.Single(o => o.Id == order.Id);
        updatedOrder?.Price.Should().Be(1000);


    }
}