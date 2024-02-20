using SQA.Domain.Exceptions;

namespace SQA.EntityFramework.Exceptions;

public class UnableToLoadUserRoleException : DomainException
{
    public string Username { get; init; }

    public UnableToLoadUserRoleException(string username)
    {
        Username = username;
    }
}