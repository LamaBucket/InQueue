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

    private readonly IUserBuilder _userBuilder;

    private readonly IUserPasswordProvider _passwordProvider;

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
            var userItem = await dbContext.Set<UserItem>().Include(x => x.Role).FirstOrDefaultAsync(x => x.Username == username);

            if (userItem is null)
                throw new Exception();

            if (userItem.Role is null)
                throw new Exception();

            UserRoleItem roleItem = userItem.Role;

            UserRole role = new(roleItem.Id, roleItem.Name, roleItem.CanManageUsers, roleItem.CanManageQueues);

            User user = _userBuilder.CreateUser(userItem.FullName, userItem.Username, role, userItem.PasswordHash);

            return user;
        }
    }

    public async Task Update(User user)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            string passwordHash = _passwordProvider.GetPasswordHash(user);

            UserItem userItem = new(user.FullName, user.Username, passwordHash, user.Role.Id);

            dbContext.Set<UserItem>().Update(userItem);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Create(string username, string fullName, string password, int roleId)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            string passwordHash = _passwordHasher.HashString(password);

            var userItem = new UserItem(fullName, username, passwordHash, roleId);

            await dbContext.Set<UserItem>().AddAsync(userItem);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<User>> Get()
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var userItems = await dbContext.Set<UserItem>().Include(x => x.Role).ToListAsync();

            List<User> users = new();

            foreach (var userItem in userItems)
            {
                if (userItem.Role is null)
                    throw new Exception();

                UserRoleItem roleItem = userItem.Role;
                UserRole userRole = new(roleItem.Id, roleItem.Name, roleItem.CanManageUsers, roleItem.CanManageQueues);

                User user = _userBuilder.CreateUser(userItem.FullName, userItem.Username, userRole, userItem.PasswordHash);

                users.Add(user);
            }

            return users;
        }
    }

    public UserDataService(SQADbContextFactory contextFactory,
                           IStringHasher passwordHasher,
                           IUserBuilder userBuilder,
                           IUserPasswordProvider passwordProvider)
    {
        _contextFactory = contextFactory;
        _passwordHasher = passwordHasher;
        _userBuilder = userBuilder;
        _passwordProvider = passwordProvider;
    }
}