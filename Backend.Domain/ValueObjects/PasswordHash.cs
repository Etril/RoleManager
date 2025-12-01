using System.Dynamic;

namespace Backend.Domain.ValueObjects;

public class PasswordHash
{
    public string Value { get;}

    public PasswordHash (string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        throw new ArgumentException("Password cannot be blank");
        Value=value;
    }
}