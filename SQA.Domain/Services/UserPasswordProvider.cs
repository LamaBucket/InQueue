namespace SQA.Domain.Services;

public class UserPasswordProvider : IUserPasswordProvider
{
    public string GetPasswordHash(User user)
    {
        return user.GetPasswordHash();
    }
}