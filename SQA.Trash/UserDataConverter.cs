using SQA.Domain;
using SQA.Domain.Services;
using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services;

public class UserDataConverter : IDataConverter<UserItem, User>
{
    private readonly IUserBuilder _userBuilder;

    private readonly IUserPasswordProvider _userPasswordProvider;

    public User Convert(UserItem item)
    {
        return _userBuilder.CreateUser(item.FullName, item.Username, item.PasswordHash);
    }

    public UserItem Convert(User item)
    {
        string passwordHash = _userPasswordProvider.GetPasswordHash(item);

        return new UserItem(item.FullName, item.Username, passwordHash);
    }

    public UserDataConverter(IUserBuilder userBuilder, IUserPasswordProvider userPasswordProvider)
    {
        _userBuilder = userBuilder;
        _userPasswordProvider = userPasswordProvider;
    }
}