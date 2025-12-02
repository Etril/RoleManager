using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.VisualBasic;

namespace Domain.Tests
{
    public class DeleteOrderTests
    {
        [Fact]

        public void DeleteOwnOrder_Suceeds_WhenUserHasPermission ()
        {
            //Arrange
            var baseRole= RoleRights.CreateBaseUser();
            var user= new User(new UserName("Test123"), new PasswordHash("hashedpassword"), baseRole);
            var orderToDelete= new Order(new OrderName("Laptop"), 1200, new OrderDate(DateTime.UtcNow));
            user.AddOrder(orderToDelete, user);

            //Act
            user.DeleteOrder(orderToDelete.Id, user);


            //Assert
            user.Orders.Should().NotContain(orderToDelete);

        }

        [Fact]
         
        public void DeleteAnyOrder_Succeeds_WhenUserHasPermission()
        {
            //Arrange
            var baseRole= RoleRights.CreateBaseUser();
            var user= new User(new UserName("Test123"), new PasswordHash("hashedpassword"), baseRole);
            var orderToDelete= new Order(new OrderName("Laptop"), 1200, new OrderDate(DateTime.UtcNow));
            user.AddOrder(orderToDelete, user);

            var managerRole= RoleRights.CreateManager();
            var userManager= new User(new UserName("TestManager123"), new PasswordHash("hashedpassword"), managerRole);

            //Act
            user.DeleteOrder(orderToDelete.Id, userManager);

            //Assert
            user.Orders.Should().NotContain(orderToDelete);


        }

        [Fact]

        public void DeleteAnyOrder_Throws_WhenUserHasNoPermission()
        {
            //Arrange
            var baseRole= RoleRights.CreateBaseUser();
            var user= new User(new UserName("Test123"), new PasswordHash("hashedpassword"), baseRole);
            var orderToDelete= new Order(new OrderName("Laptop"), 1200, new OrderDate(DateTime.UtcNow));
            user.AddOrder(orderToDelete, user);

            var noPermissionRole= new Role(RoleType.BaseUser, new HashSet<Permission>());
            var userNoPermission= new User(new UserName("TestManager123"), new PasswordHash("hashedpassword"), noPermissionRole);

            //Act
            Action act= () => user.DeleteOrder(orderToDelete.Id, userNoPermission);


            //Assert
            act.Should().Throw<UnauthorizedAccessException>();
        }
    
}
}