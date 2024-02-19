using Microsoft.EntityFrameworkCore;
using SQA.Domain;
using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services;

public class UserRoleDataService : IUserRoleDataService
{
    private readonly SQADbContextFactory _contextFactory;

    public async Task Create(string name, bool canManageUsers, bool canManageQueues)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            UserRoleItem roleItem = new(name, canManageUsers, canManageQueues);

            await dbContext.Set<UserRoleItem>().AddAsync(roleItem);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Delete(int roleId)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var role = await dbContext.Set<UserRoleItem>().FirstOrDefaultAsync(x => x.Id == roleId);

            if (role is null)
                throw new Exception();

            dbContext.Set<UserRoleItem>().Remove(role);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<UserRole>> GetAllRoles()
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var roleItems = await dbContext.Set<UserRoleItem>().ToListAsync();

            List<UserRole> roles = new();

            foreach (var roleItem in roleItems)
            {
                UserRole role = new(roleItem.Id, roleItem.Name, roleItem.CanManageUsers, roleItem.CanManageQueues);

                roles.Add(role);
            }

            return roles;
        }
    }

    public UserRoleDataService(SQADbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }
}