using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.VisualBasic;

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
    


}
}