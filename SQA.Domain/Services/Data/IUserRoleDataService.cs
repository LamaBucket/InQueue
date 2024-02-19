namespace SQA.Domain;

public interface IUserRoleDataService
{
    Task<IEnumerable<UserRole>> GetAllRoles();

    Task Create(string name, bool canManageUsers, bool canManageQueues);

    Task Delete(int roleId);
}