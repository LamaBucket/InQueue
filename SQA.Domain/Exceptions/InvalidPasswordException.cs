using System.Resources;

namespace SQA.Domain.Exceptions;

public class InvalidPasswordException : DomainException
{
    public string Username { get; init; }

    public InvalidPasswordException(string username)
    {
        Username = username;
    }
}