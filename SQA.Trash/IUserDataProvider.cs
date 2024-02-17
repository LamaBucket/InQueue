using SQA.Domain;

namespace SQA.Domain.Services;


public interface IUserDataProvider : IDataProvider<User>
{
    Task<User> Get(string username);
}