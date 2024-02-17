using SQA.Domain.Services;

namespace SQA.Domain;

public class User : DomainObject
{

    public string FullName { get; internal set; }

    public string Username { get; init; }

    private string _passwordHash;

    private readonly IStringHasher _passwordHasher;


    public bool ValidatePassword(string password)
    {
        return _passwordHasher.HashString(password).Equals(_passwordHash);
    }

    public void UpdatePassword(string oldPassword, string newPassword)
    {
        if (ValidatePassword(oldPassword))
        {
            string newPasswordHash = _passwordHasher.HashString(newPassword);
            _passwordHash = newPasswordHash;
        }
    }

    public void UpdateFullName(string password, string name)
    {
        if (ValidatePassword(password))
        {
            FullName = name;
        }
    }

    internal string GetPasswordHash()
    {
        return _passwordHash;
    }

    internal User(string fullName,
                  string username,
                  string passwordHash,
                  IStringHasher passwordHasher)
    {
        FullName = fullName;
        Username = username;

        _passwordHash = passwordHash;

        _passwordHasher = passwordHasher;
    }
}
