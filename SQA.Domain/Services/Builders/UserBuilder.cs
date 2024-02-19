namespace SQA.Domain.Services;

public class UserBuilder : IUserBuilder
{
    public IStringHasher PasswordHasher { get; init; }

    public User CreateUser(string fullName, string username, UserRole role, string passwordHash)
    {
        return new User(fullName, username, passwordHash, role, PasswordHasher);
    }

    public UserBuilder(IStringHasher passwordHasher)
    {
        PasswordHasher = passwordHasher;
    }
}
