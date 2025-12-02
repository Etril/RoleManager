using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using Backend.Domain.Services;
using FluentAssertions;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;

namespace Domain.Tests
{
    public class UserEntityTests
    {
        [Fact]
        
        public void User_Creation_Succeeds_WhenConditionsAreMet()
        {
            //Arrange
            var baseRole= RoleRights.CreateBaseUser();
            var userName= new UserName("Laptop");
            var hashedPassword= new PasswordHash("hashedpassword");

            //Act
            var user= new User(userName, hashedPassword, baseRole);

            //Assert
            user.Role.Should().Be(baseRole);
            user.Username.Should().Be(userName);
            user.Password.Should().Be(hashedPassword);
            user.Should().NotBeNull();
            
        }

        [Fact]

        public void User_Creation_Fails_WhenUserNameIsEmpty() {
        
        //Arrange
        var baseRole= RoleRights.CreateBaseUser();
        var hashedPassword= new PasswordHash("hashedpassword");

        //Act
        Action act= ()=> new User(new UserName(""), hashedPassword, baseRole);

        //Assert
        act.Should().Throw<ArgumentException>();
        
        }

        [Fact]

        public void User_Creation_Fails_WhenPasswordIsEmpty()
        {
            //Arrange
            var baseRole= RoleRights.CreateBaseUser();
            var userName= new UserName("Laptop");

            //Act
            Action act= () => new User(userName, new PasswordHash(""), baseRole );

            //Assert
            act.Should().Throw<ArgumentException>();
            
        }

        [Fact]

        public void UpdateUser_UpdatesCorrectly_WhenConditionsAreMet()
        {
            //Arrange
            var user= new User(new UserName("test"), new PasswordHash("hashedpassword"), RoleRights.CreateBaseUser());
            var userAdmin= new User(new UserName("test"), new PasswordHash("hashedpassword"), RoleRights.CreateAdmin());
            var service= new UserManagementService();

            //Act
            service.UpdateUser(user, new UserName("change"), new PasswordHash("hashedpassword2"), RoleRights.CreateManager(), userAdmin);

            //Assert
            user.Username.Value.Should().Be("change");
            user.Password.Value.Should().Be("hashedpassword2");
            user.Role.Type.Should().Be(RoleType.Manager);

        }

        [Fact]

        public void UpdateUser_Throws_WhenNoPermissions()
        {
            //Arrange
            var user= new User(new UserName("test"), new PasswordHash("hashedpassword"), RoleRights.CreateBaseUser());
            var userManager= new User(new UserName("test"), new PasswordHash("hashedpassword"), RoleRights.CreateManager());
            var service= new UserManagementService();

            //Act
            Action act = () => service.UpdateUser(user, new UserName("change"), new PasswordHash("hashedpassword2"), RoleRights.CreateManager(), userManager);

            //Assert
            act.Should().Throw<UnauthorizedAccessException>();


        }


}
}