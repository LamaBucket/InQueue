using SQA.Domain.Exceptions;

namespace SQA.EntityFramework.Exceptions;

public class UserAlreadyExistsException : DomainException
{
    public string Username { get; init; }

    public UserAlreadyExistsException(string username)
    {
        Username = username;
    }
}