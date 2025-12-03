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
    public class CreateOrderCommandHandlerTests
    {

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;

        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
         _userRepositoryMock= new Mock<IUserRepository>();
         _orderRepositoryMock= new Mock<IOrderRepository>();

         _handler= new CreateOrderCommandHandler(
            _userRepositoryMock.Object,
            _orderRepositoryMock.Object
         )   ;
        }

        [Fact]

        public async Task CreateOrderCommandHandler_Succeeds_WhenValid()
        {
            //Arrange
            var userId= Guid.NewGuid();
            var command= new CreateOrderCommand("Test", 50, DateTime.UtcNow);

            var fakeUser= new User (new UserName("TestUser"), new PasswordHash("hashedpassword"), RoleRights.CreateBaseUser());

            _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(fakeUser);

            _orderRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

            //Act

            var result= await _handler.Handle(command, userId);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(command.Name);
            result.Price.Should().Be(command.Price);
            result.Date.Should().Be(command.Date);
        }

        [Fact]

        public async Task CreateOrderCommandHandler_Throws_WhenUserNotFound()
        {
            //Arrange
            var userId= Guid.NewGuid();
            var command= new CreateOrderCommand("Test", 50, DateTime.UtcNow);

            //Act
            Func<Task> act = async () => await _handler.Handle(command, userId);

            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("User not found");
        
        }

        [Fact]
        
        public async Task CreateOrderCommandHandler_CallsOrderRepository_OnlyOnce()
        {
            //Arrange
            var userId= Guid.NewGuid();
            var command= new CreateOrderCommand("Test", 50, DateTime.UtcNow);

            var fakeUser= new User (new UserName("TestUser"), new PasswordHash("hashedpassword"), RoleRights.CreateBaseUser());

            _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(fakeUser);

            _orderRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

            //Act
             var result= await _handler.Handle(command, userId);

             //Assert

             _orderRepositoryMock.Verify(
                repo => repo.AddAsync(It.Is<Order>(o => 
                o.Name.Value == command.Name &&
                o.Price == command.Price &&
                o.Date.Value == command.Date)),
                Times.Once
             );


        }
    }

    
}