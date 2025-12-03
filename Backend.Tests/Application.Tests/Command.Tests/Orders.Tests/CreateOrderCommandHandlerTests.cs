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

        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
         _userRepositoryMock= new Mock<IUserRepository>();

         _handler= new CreateOrderCommandHandler(
            _userRepositoryMock.Object
         )   ;
        }

        [Fact]

        public async Task CreateOrderCommandHandler_Succeeds_WhenValid()
        {
            //Arrange
            var targetUserId= Guid.NewGuid();
            var createdByUserId= Guid.NewGuid();

            var command= new CreateOrderCommand("Test", 50, DateTime.UtcNow);

            var targetUser= new User (new UserName("TestUser"), new PasswordHash("hashedpassword"), RoleRights.CreateBaseUser());
            var createdByUser= new User(new UserName("Manager"), new PasswordHash("hashedpassword"), RoleRights.CreateManager());


            _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(targetUserId))
            .ReturnsAsync(targetUser);

            _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(createdByUserId))
            .ReturnsAsync(createdByUser);


            //Act

            var result= await _handler.Handle(command, targetUserId, createdByUserId);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]

        public async Task CreateOrderCommandHandler_Throws_WhenUserNotFound()
        {
            //Arrange
            var userId= Guid.NewGuid();
            var command= new CreateOrderCommand("Test", 50, DateTime.UtcNow);

            //Act
            Func<Task> act = async () => await _handler.Handle(command, userId, userId);

            //Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("User not found");
        
        }

        [Fact]
        
        public async Task CreateOrderCommandHandler_CallsOrderRepository_OnlyOnce()
        {
            //Arrange
            var targetUserId= Guid.NewGuid();
            var createdByUserId= Guid.NewGuid();

            var command= new CreateOrderCommand("Test", 50, DateTime.UtcNow);

            var targetUser= new User (new UserName("TestUser"), new PasswordHash("hashedpassword"), RoleRights.CreateBaseUser());
            var createdByUser= new User(new UserName("Manager"), new PasswordHash("hashedpassword"), RoleRights.CreateManager());

            _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(targetUserId))
            .ReturnsAsync(targetUser);

            _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(createdByUserId))
            .ReturnsAsync(createdByUser);

            //Act
             var result= await _handler.Handle(command, targetUserId, createdByUserId);

             //Assert

             _userRepositoryMock.Verify(
                repo => repo.UpdateAsync(targetUser),
                Times.Once
             );


        }
    }

    
}