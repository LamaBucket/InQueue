using SQA.Domain.Services;

namespace SQA.Web;

public class PasswordHasher : IStringHasher
{
    public string HashString(string input)
    {
        return String.Join('|', input.Reverse());
    }
}