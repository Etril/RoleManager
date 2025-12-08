using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;
using Backend.Application;
using FluentAssertions;
using Microsoft.VisualBasic;
using Moq;
using Backend.Application.Repositories;
using Backend.Application.Commands;
using Backend.Domain.Roles;
using FluentAssertions.Common;
using System.Threading.Tasks;

namespace Application.Tests;

public class DeleteOrderCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;

    private readonly DeleteOrderCommandHandler _handler;

    public DeleteOrderCommandHandlerTests()
    {
        _userRepositoryMock= new Mock<IUserRepository>();

        _handler = new DeleteOrderCommandHandler(
            _userRepositoryMock.Object
        );
    }

    [Fact]

    public async Task DeleteOrderCommandHandler_Succeeds_WhenValid()
    {
        //Arrange
        var targetUserId= Guid.NewGuid();
        var deletedByUserId= Guid.NewGuid();

        var order = new Order(new OrderName("Test"), 100, new OrderDate(DateTime.UtcNow));

        var command= new DeleteOrderCommand(order.Id);

        var targetUser= new User (new UserName("Testuser"), PasswordHash.FromHash("hashed"), RoleRights.CreateBaseUser());
        targetUser.AddOrder(order, targetUser);

        var deletedByUser= new User(new UserName("Testuser"), PasswordHash.FromHash("hashed"), RoleRights.CreateManager());

        _userRepositoryMock
        .Setup(repo => repo.GetByOrderIdAsync(order.Id))
        .ReturnsAsync(targetUser);

        _userRepositoryMock
        .Setup(repo => repo.GetByIdAsync(deletedByUserId))
        .ReturnsAsync(deletedByUser);

        //Act
        var result= await _handler.Handle(command, deletedByUserId);

        //Assert
        result.Message.Should().Be("Order deleted");
        result.Success.Should().Be(true);

    }

    [Fact]

    public async Task DeleteOrderCommandHandler_Throws_WhenOwnerNotFound()
    {
        //Arrange
        var targetUserId= Guid.NewGuid();
         var order= new Order(new OrderName("test"), 10, new OrderDate(DateTime.UtcNow));
         var command = new DeleteOrderCommand(order.Id);
        //Act
        Func<Task> act = async () => await _handler.Handle(command, targetUserId);

        //Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Order owner not found");

    }

    [Fact]

    public async Task DeleteOrderCommandHandler_Throws_WhenActorUnauthorized()
    {
        //Arrange
        var targetUserId= Guid.NewGuid();
        var deletedByUserId= Guid.NewGuid();
        var password= PasswordHash.FromHash("hashed");

        var order = new Order(new OrderName("Test"), 100, new OrderDate(DateTime.UtcNow));

        var command= new DeleteOrderCommand(order.Id);

        var targetUser= new User (new UserName("Testuser"), PasswordHash.FromHash("hashed"), RoleRights.CreateBaseUser());
        targetUser.AddOrder(order, targetUser);

        var deletedByUser= new User(new UserName("Testuser123"), PasswordHash.FromHash("hashed"), RoleRights.CreateBaseUser());

        _userRepositoryMock
        .Setup(repo => repo.GetByOrderIdAsync(order.Id))
        .ReturnsAsync(targetUser);

        _userRepositoryMock
        .Setup(repo => repo.GetByIdAsync(deletedByUserId))
        .ReturnsAsync(deletedByUser);

        //Act 
        Func<Task> act = async () => await _handler.Handle(command, deletedByUserId);

        //Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();


    }

}