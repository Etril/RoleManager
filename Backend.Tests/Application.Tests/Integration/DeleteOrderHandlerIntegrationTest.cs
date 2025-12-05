using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Application;
using Backend.Application.Commands;
using Backend.Domain.Entities;
using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using Backend.Infrastructure.Persistence;
using Backend.Infrastructure.Persistence.Repositories;
using Backend.Tests.Common;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Integration;

public class DeleteOrderHandlerIntegrationTests
{
    [Fact]
    
    public async Task DeleteOrderHandlerIntegratesProperly()
    {
        
        //Arrange
        using var context = TestDbContextFactory.Create();

        var repo= new UserRepository(context);

        var user= new User(new UserName("Test123"), new PasswordHash("hashed"), RoleRights.CreateBaseUser());
        var order= new Order (new OrderName("Laptop"), 100, new OrderDate(DateTime.UtcNow));

        user.AddOrder(order, user);

        context.Users.Add(user);

        var admin = new User (new UserName("Admin"), new PasswordHash("hashed"), RoleRights.CreateAdmin());
        context.Users.Add(admin);

        await context.SaveChangesAsync();

        var handler= new DeleteOrderCommandHandler(repo);
        var command= new DeleteOrderCommand(order.Id);

        //Act
        var response= await handler.Handle(command, admin.Id);



        //Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Order deleted");

        
    }
}