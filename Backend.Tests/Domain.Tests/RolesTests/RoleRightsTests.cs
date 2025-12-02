using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;
using Backend.Domain.Roles;
using FluentAssertions;
using Microsoft.VisualBasic;

namespace Domain.Tests
{
    public class RolesRightsTests
    {
        [Fact]

        public void CreateBaseUser_ShouldReturnRole_WithCorrectPermissions()
        {
            //Act
            var role = RoleRights.CreateBaseUser();

            //Assert
            role.Type.Should().Be(RoleType.BaseUser);
            role.Permissions.Should().BeEquivalentTo(new[]
            {
                Permission.EditOwnOrder,
                Permission.DeleteOwnOrder,
                Permission.AddOrder
            });

        }

        [Fact]

        public void CreateManager_ShouldReturnRole_WithCorrectPermissions()
        {
            //Act
            var role = RoleRights.CreateManager();

            //Assert
            role.Type.Should().Be(RoleType.Manager);
            role.Permissions.Should().BeEquivalentTo(new[]
            {
                Permission.EditOwnOrder,
                 Permission.EditAnyOrder,
                 Permission.DeleteAnyOrder,
                 Permission.DeleteOwnOrder,
                 Permission.AddOrder
            });
        }

        [Fact]
        public void CreateAdmin_ShouldReturnRole_WithCorrectPermissions()
        {
            //Act
            var role = RoleRights.CreateAdmin();

            //Assert
            role.Type.Should().Be(RoleType.Admin);
            role.Permissions.Should().BeEquivalentTo(new[]
            {
                Permission.EditAnyOrder,
                Permission.EditOwnOrder,
                Permission.DeleteAnyOrder,
                Permission.DeleteOwnOrder,
                Permission.AddOrder,
                Permission.EditUsers
            });
        }
    }
}