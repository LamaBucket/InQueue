using Microsoft.EntityFrameworkCore;
using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services.DataTable;

public class UserItemProvider : IUserItemProvider
{
    private readonly SQADbContextFactory _dbContextFactory;


    public async Task<UserItem> Get(string username)
    {
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            var item = await dbContext.Set<UserItem>().FirstOrDefaultAsync(x => x.Username == username);

            if (item is null)
                throw new Exception();

            return item;
        }
    }

    public async Task<IEnumerable<UserItem>> GetAll()
    {
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            var items = await dbContext.Set<UserItem>().ToListAsync();

            return items;
        }
    }

    public UserItemProvider(SQADbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
}