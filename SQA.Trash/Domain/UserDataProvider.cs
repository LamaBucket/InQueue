using SQA.Domain;
using SQA.Domain.Services;
using SQA.EntityFramework.Model;
using SQA.EntityFramework.Services.DataTable;

namespace SQA.EntityFramework.Services.Domain;

public class UserDataProvider : IUserDataProvider
{
    private readonly IUserItemProvider _userItemProvider;

    private readonly IDataConverter<UserItem, User> _converter;

    public async Task<User> Get(string username)
    {
        UserItem userObject = await _userItemProvider.Get(username);

        User user = _converter.Convert(userObject);

        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        IEnumerable<UserItem> userObjects = await _userItemProvider.GetAll();

        List<User> users = new();

        foreach (UserItem userObject in userObjects)
        {
            User user = _converter.Convert(userObject);

            users.Add(user);
        }

        return users;
    }

    public UserDataProvider(IUserItemProvider userItemProvider,
                           IDataConverter<UserItem, User> converter)
    {
        _userItemProvider = userItemProvider;
        _converter = converter;
    }
}