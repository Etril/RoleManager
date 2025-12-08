using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.VisualBasic;

namespace Domain.Tests
{
    public class EditOrderTests
    {
        [Fact]

        public void EditOwnOrder_Succeeds_WhenUserHasPermission ()
        {
            //Arrange
            var baseRole= RoleRights.CreateBaseUser();
            var user= new User (new UserName("Test123"), PasswordHash.FromHash("hashed"), baseRole);
            var orderToEdit= new Order (new OrderName("Laptop"), 100, new OrderDate(DateTime.UtcNow));
            user.AddOrder(orderToEdit, user);

            //Act
            user.UpdateOrderPrice(orderToEdit.Id, 1000, user);

            //Assert
            orderToEdit.Price.Should().Be(1000);
            
        }

        [Fact]

        public void EditAnyOrder_Succeeds_WhenUserHasPermission ()
        {
            //Arrange
            var baseRole= RoleRights.CreateBaseUser();
            var user= new User (new UserName("Test123"), PasswordHash.FromHash("hashed"), baseRole);
            var orderToEdit= new Order (new OrderName("Laptop"), 100, new OrderDate(DateTime.UtcNow));
            user.AddOrder(orderToEdit, user);

            var managerRole= RoleRights.CreateManager();
            var userManager= new User(new UserName("Manager123"), PasswordHash.FromHash("hashed"), managerRole);

            //Act
           user.UpdateOrderPrice(orderToEdit.Id, 1000, userManager);

            //Assert
            orderToEdit.Price.Should().Be(1000);
            
        }

        [Fact]

        public void EditAnyOrder_Throws_WhenUserHasNoPermission ()
        {
            //Arrange
            var baseRole= RoleRights.CreateBaseUser();
            var user= new User (new UserName("Test123"), PasswordHash.FromHash("hashed"), baseRole);
            var orderToEdit= new Order (new OrderName("Laptop"), 100, new OrderDate(DateTime.UtcNow));
            user.AddOrder(orderToEdit, user);

            var noPermissionRole= new Role (RoleType.BaseUser, new HashSet<Permission>());
            var userNoPermission= new User(new UserName("Manager123"), PasswordHash.FromHash("hashed"), noPermissionRole);


            //Act
            Action act= () => user.UpdateOrderPrice(orderToEdit.Id, 1000, userNoPermission);

            //Assert
            act.Should().Throw<UnauthorizedAccessException>();
        }

    }
}