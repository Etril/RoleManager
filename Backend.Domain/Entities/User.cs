using Backend.Domain.ValueObjects;
using Backend.Domain.Roles;
using Microsoft.VisualBasic;

namespace Backend.Domain.Entities;

public class User
{
    public Guid Id {get; private set;}
    public UserName Username {get; private set;}

    public PasswordHash Password {get; private set;}

    public Role Role { get; private set; }

    public User (UserName username, PasswordHash password, Role role)
    {
        Id= Guid.NewGuid();
        Username= username;
        Password= password;
        Role= role ?? throw new ArgumentNullException(nameof(role));
    }
}