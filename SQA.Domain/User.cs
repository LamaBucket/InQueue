using SQA.Domain.Exceptions;
using SQA.Domain.Services;

namespace SQA.Domain;

public class User : DomainObject
{

    public string FullName { get; set; }

    public string Username { get; init; }

    public UserRole Role { get; set; }


    private string _passwordHash;

    private readonly IStringHasher _passwordHasher;


    public bool ValidatePassword(string password)
    {
        return _passwordHasher.HashString(password).Equals(_passwordHash);
    }

    public void UpdatePassword(string oldPassword, string newPassword)
    {
        if (!ValidatePassword(oldPassword))
            throw new InvalidPasswordException(Username);

        string newPasswordHash = _passwordHasher.HashString(newPassword);
        _passwordHash = newPasswordHash;
    }

    internal string GetPasswordHash()
    {
        return _passwordHash;
    }

    internal User(string fullName,
                  string username,
                  string passwordHash,
                  UserRole role,
                  IStringHasher passwordHasher)
    {
        FullName = fullName;
        Username = username;
        Role = role;

        _passwordHash = passwordHash;

        _passwordHasher = passwordHasher;
    }
}
