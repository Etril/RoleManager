using System.Security;

namespace Backend.Domain.Roles; 

public enum RoleType
{
    BaseUser,
    Manager,
    Admin
}
public class Role
{
    public RoleType Type { get; }
    public HashSet<Permission> Permissions { get; }

    public Role(RoleType type, HashSet<Permission> permissions)
    {
        Type= type;
        Permissions= permissions;
    }

}