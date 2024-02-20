using SQA.Domain.Exceptions;

namespace SQA.EntityFramework.Exceptions;

public class RoleDoesNotExistException : DomainException
{
    public int RoleId { get; init; }

    public RoleDoesNotExistException(int roleId)
    {
        RoleId = roleId;
    }
}