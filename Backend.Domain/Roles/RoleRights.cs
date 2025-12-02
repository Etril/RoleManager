namespace Backend.Domain.Roles;
using System.Collections.Generic;

public static class RoleRights
{
    public static Role CreateBaseUser()
    => new Role(RoleType.BaseUser, new HashSet<Permission>
    {
        Permission.EditOwnOrder,
        Permission.DeleteOwnOrder,
        Permission.AddOrder
    });

    public static Role CreateManager()
    => new Role(RoleType.Manager, new HashSet<Permission>
    {
        Permission.EditOwnOrder,
        Permission.EditAnyOrder,
        Permission.DeleteAnyOrder,
        Permission.DeleteOwnOrder,
        Permission.AddOrder
    });

    public static Role CreateAdmin()
    => new Role(RoleType.Admin, new HashSet<Permission>
    {
        Permission.EditAnyOrder,
        Permission.EditOwnOrder,
        Permission.EditUsers,
        Permission.DeleteAnyOrder,
        Permission.DeleteOwnOrder,
        Permission.AddOrder
    });
}