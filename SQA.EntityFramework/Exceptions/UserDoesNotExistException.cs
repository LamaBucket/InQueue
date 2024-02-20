using SQA.Domain.Exceptions;

namespace SQA.EntityFramework.Exceptions;

public class UserDoesNotExistException : DomainException
{
    public string Username { get; init; }

    public UserDoesNotExistException(string username)
    {
        Username = username;
    }
}