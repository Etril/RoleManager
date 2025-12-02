using Backend.Domain.Entities;
using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;

namespace Backend.Domain.Services;

public class UserManagementService
{
    public void UpdateUser (
        User target, 
        UserName newUserName,
        PasswordHash newPasswordHash,
        Role newRole,
        User updatedBy
    )
    {
        if (!updatedBy.Role.Permissions.Contains(Permission.EditUsers))
        throw new UnauthorizedAccessException();

        if (newRole.Type == RoleType.Admin)
        throw new InvalidOperationException("Cannot give admin role");

        target.UpdateUserDetails(newUserName, newPasswordHash, newRole);

    }
}