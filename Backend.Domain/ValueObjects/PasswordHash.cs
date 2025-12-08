using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using BCrypt.Net;

namespace Backend.Domain.ValueObjects;

public class PasswordHash
{
    public string? Value { get; private set;}

    private PasswordHash() {}

    public static PasswordHash FromPlainText(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        throw new ArgumentException("Password cannot be blank");


        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        return new PasswordHash {Value = hash};
    }

    public static PasswordHash FromHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Hash cannot be blank");

        return new PasswordHash { Value = hash };
    }
    
    public bool Matches(string plaintext) =>
    BCrypt.Net.BCrypt.Verify(plaintext, Value);
}