namespace SQA.Domain.Services;

public interface IUserPasswordProvider
{
    string GetPasswordHash(User user);
}