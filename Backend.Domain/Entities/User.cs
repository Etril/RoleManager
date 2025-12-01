using Backend.Domain.ValueObjects;

namespace Backend.Domain.Entities;

public class User
{
    public Guid Id {get; private set;}
    public UserName Username {get; private set;}

    public PasswordHash Password {get; private set;}

    public User (UserName username, PasswordHash password)
    {
        Id= Guid.NewGuid();
        Username= username;
        Password= password;
    }
}