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

namespace Application.Tests
{
    public class EditOrderCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;

        private readonly EditOrderCommandHandler _handler;

        public EditOrderCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            _handler= new EditOrderCommandHandler(
                _userRepositoryMock.Object
            );
        }

        [Fact]

        public async Task EditOrderCommandHandler_Succeeds_WhenValid()
        {
            //Arrange
            var targetUserId= Guid.NewGuid();
            var editedByUserId= Guid.NewGuid();
            var order= new Order(new OrderName("test"), 10, new OrderDate(DateTime.UtcNow));

            var command =new EditOrderCommand(order.Id, order.Name.Value, 100, order.Date.Value);

            var targetUser= new User (new UserName("Testuser"), new PasswordHash("hashedpassword"), RoleRights.CreateBaseUser());
            targetUser.AddOrder(order, targetUser);
            var editedByUser= new User(new UserName("Manager"), new PasswordHash("hashedpassword"), RoleRights.CreateManager());

            _userRepositoryMock
            .Setup(repo => repo.GetByOrderIdAsync(order.Id))
            .ReturnsAsync(targetUser);

            _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(editedByUserId))
            .ReturnsAsync(editedByUser);

            //Act
            var result= await _handler.Handle(command, order.Id, editedByUserId );

            //Assert
            result.Should().NotBeNull();
            result.Price.Should().Be(100);

        }

        [Fact]

        public async Task EditOrdercommandHandler_Throws_WhenOwnerNotFound()
        {
            //Arrange
            var targetUserId= Guid.NewGuid();
            var order= new Order(new OrderName("test"), 10, new OrderDate(DateTime.UtcNow));
            var command = new EditOrderCommand(order.Id, order.Name.Value, 100, order.Date.Value);

            //Act
            Func<Task> act= async () => await _handler.Handle(command, targetUserId, targetUserId);


            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Order owner not found");
        }

        [Fact]

        public async Task EditOrderCommandHandler_Throws_WhenActorUnauthorized()
        {
            //Arrange
            var targetUserId= Guid.NewGuid();
            var editedByUserId= Guid.NewGuid();
            var order= new Order(new OrderName("test"), 10, new OrderDate(DateTime.UtcNow));

            var command =new EditOrderCommand(order.Id, order.Name.Value, 100, order.Date.Value);

            var targetUser= new User (new UserName("Testuser"), new PasswordHash("hashedpassword"), RoleRights.CreateBaseUser());
            targetUser.AddOrder(order, targetUser);
            
            var editedByUser= new User(new UserName("Test1234"), new PasswordHash("hashedpassword"), RoleRights.CreateBaseUser());

            _userRepositoryMock
            .Setup(repo => repo.GetByOrderIdAsync(order.Id))
            .ReturnsAsync(targetUser);

            _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(editedByUserId))
            .ReturnsAsync(editedByUser);

            //Act
            Func<Task> act = async () => await _handler.Handle(command, order.Id, editedByUserId);

            //Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
}
}