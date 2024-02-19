namespace SQA.Domain.Services;

public interface IUserBuilder
{
    IStringHasher PasswordHasher { get; }

    User CreateUser(string fullName, string username, UserRole role, string passwordHash);
}