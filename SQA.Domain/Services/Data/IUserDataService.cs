using SQA.Domain;

namespace SQA.Domain.Services.Data;

public interface IUserDataService
{
    Task<IEnumerable<User>> Get();

    Task<User> Get(string username);

    Task Create(string username, string fullName, string password);

    Task Update(User user);

    Task Delete(string username);
}