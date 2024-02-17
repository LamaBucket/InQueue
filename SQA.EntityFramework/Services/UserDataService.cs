using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SQA.Domain;
using SQA.Domain.Services;
using SQA.Domain.Services.Data;
using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services;

public class UserDataService : IUserDataService
{
    private readonly SQADbContextFactory _contextFactory;

    private readonly IDataConverter<UserItem, User> _converter;

    private readonly IStringHasher _passwordHasher;

    public async Task Delete(string username)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var userItem = await dbContext.Set<UserItem>().FirstOrDefaultAsync(x => x.Username == username);

            if (userItem is null)
                throw new Exception();

            dbContext.Set<UserItem>().Remove(userItem);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<User> Get(string username)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var userItem = await dbContext.Set<UserItem>().FirstOrDefaultAsync(x => x.Username == username);

            if (userItem is null)
                throw new Exception();

            var user = _converter.Convert(userItem);

            return user;
        }
    }

    public async Task Update(User user)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var userItem = _converter.Convert(user);

            dbContext.Set<UserItem>().Update(userItem);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Create(string username, string fullName, string password)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            string passwordHash = _passwordHasher.HashString(password);

            var userItem = new UserItem(fullName, username, passwordHash);

            await dbContext.Set<UserItem>().AddAsync(userItem);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<User>> Get()
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var userItems = await dbContext.Set<UserItem>().ToListAsync();

            var users = userItems.Select(_converter.Convert);

            return users;
        }
    }

    public UserDataService(SQADbContextFactory contextFactory,
                           IDataConverter<UserItem, User> converter,
                           IStringHasher passwordHasher)
    {
        _contextFactory = contextFactory;
        _converter = converter;
        _passwordHasher = passwordHasher;
    }
}