using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services.DataTable;

public interface IUserItemProvider : IDataProvider<UserItem>
{
    Task<UserItem> Get(string username);
}