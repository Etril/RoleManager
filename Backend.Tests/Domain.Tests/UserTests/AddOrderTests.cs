using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.VisualBasic;

namespace Domain.Tests
{
    public class AddOrderTests
    {
        [Fact]

        public void AddOrder_Succeeds_WhenUserHasPermission ()
        {
            //Arrange
            var baseRole = RoleRights.CreateBaseUser();
            var user= new User(new UserName("Test123"), new PasswordHash("hashedPassword"), baseRole);
            var order= new Order (new OrderName("Laptop"), 100, new OrderDate(DateTime.UtcNow));

            //Act
            user.AddOrder(order, user);

            //Assert

            user.Orders.Should().Contain(order);
        }

        [Fact]

        public void AddOrder_Throws_WhenUserLacksPermission ()
        {
            //Arrange
            var noPermissionRole = new Role(RoleType.BaseUser, new HashSet<Permission>());
            var user= new User(new UserName("Test123"), new PasswordHash("hashedPassword"), noPermissionRole);
            var order= new Order (new OrderName("Laptop"), 100, new OrderDate(DateTime.UtcNow));


            //Act
            Action act = () => user.AddOrder(order, user);

            //Assert
            act.Should().Throw<UnauthorizedAccessException>();
        }
    }
}