using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;
using Backend.Domain.Roles;
using FluentAssertions;
using Microsoft.VisualBasic;

namespace Domain.Tests
{
    public class RolesTests
    {
        [Fact]

        public void Role_Creation_SetsTypeAndPermissions()
        {
            //Arrange 
            var type= RoleType.BaseUser;
            var permissions= new HashSet<Permission> {Permission.EditOwnOrder};

            //Act
            var role= new Role(type, permissions);

            //Assert
            role.Type.Should().Be(RoleType.BaseUser);
            role.Permissions.Should().Contain(Permission.EditOwnOrder);

        }
    }
}
