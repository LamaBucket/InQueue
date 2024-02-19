namespace SQA.Domain.Exceptions;

public interface IDomainExceptionHandler<T> where T : Exception
{
    T HandleException(DomainException exception);
}