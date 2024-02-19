using System;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using SQA.Domain;
using SQA.Domain.Services;
using SQA.Domain.Services.Data;
using SQA.EntityFramework;
using SQA.EntityFramework.Model;
using SQA.EntityFramework.Services;

public class PasswordHasher : IStringHasher
{
    public string HashString(string input)
    {
        return String.Join('|', input.Reverse());
    }
}

internal class Program
{

    public static IUserDataService userDataService = null!;

    public static IQueueDataService queueDataSerivce = null!;

    public static IUserRoleDataService userRoleDataService = null!;

    public static async Task Main(string[] args)
    {
        SQADbContextFactory contextFactory = new();

        IStringHasher passwordHasher = new PasswordHasher();

        IUserBuilder userBuilder = new UserBuilder(passwordHasher);
        IQueueBuilder queueBuilder = new QueueBuilder();

        IUserPasswordProvider userPasswordProvider = new UserPasswordProvider();

        userDataService = new UserDataService(contextFactory, passwordHasher, userBuilder, userPasswordProvider);
        queueDataSerivce = new QueueDataService(contextFactory, queueBuilder);
        userRoleDataService = new UserRoleDataService(contextFactory);

        print("Welcome To Test Console App of Simple-Queueing-Application (SQA)");
        print("Type help to get the basic info of commands you can use.");

        bool flag = true;

        while (flag)
        {
            string? text = Console.ReadLine();
            if (text is null)
                continue;
            try
            {
                bool commandUsed = true;

                switch (text)
                {
                    case "help":
                        print("user.add - Create User");
                        print("user.view - Display List Of Users");
                        print("user.delete - Delete User");
                        print("user.updatePassword - Start User Updating");

                        print("queue.add - Create Queue");
                        print("queue.delete - Delete Queue");
                        print("queue.view.details - Display Details Of 1 Queue");
                        print("queue.view.info - Display Details Of Queues where some user is present.");
                        print("queue.view.info.all - Display Details Of All Queues.");
                        print("queue.addUser - Add User to Queue");
                        print("queue.removeRecord - Remove Record from Queue");
                        print("queue.moveNext - Move To The Next Person in Queue");

                        print("role.view - Display All Current Roles");
                        print("role.add - Create Role");
                        print("role.delete - Delete Role");
                        commandUsed = false;
                        break;
                    case "exit":
                        flag = false;
                        commandUsed = false;
                        break;
                    case "user.add":
                        await CreateUser();
                        break;
                    case "user.view":
                        foreach (var user in await userDataService.Get())
                        {
                            print($"> Username: {user.Username}; FullName: {user.FullName}; ");
                        }
                        break;
                    case "user.delete":
                        await DeleteUser();
                        break;
                    case "user.updatePassword":
                        await StartUserPasswordUpdate();
                        break;
                    case "user.rename":
                        await StartUserRename();
                        break;

                    case "queue.view.info":
                        string username = read("Enter Username:");

                        var infos = (await queueDataSerivce.GetForUser(username)).ToList();

                        print($"Queues Where {username} is present:");

                        for (int i = 0; i < infos.Count; i++)
                        {
                            var info = infos[i];
                            print($"> Id: {info.Id}; Name: {info.Name}; DateTime Created: {info.Created}");
                        }
                        break;
                    case "queue.view.info.all":
                        await PrintQueuesInfo();
                        break;
                    case "queue.view.details":
                        var queue = await SelectQueue();
                        var queueInfo = queue.QueueInfo;
                        var queueRecords = queue.Records.ToList();

                        print($"> Id: {queueInfo.Id}; Name: {queueInfo.Name}; DateTime Created: {queueInfo.Created}");
                        print("People In Queue:");

                        int currentPosition = queue.CurrentPosition;

                        for (int i = 0; i < queueRecords.Count; i++)
                        {
                            if (currentPosition == i)
                            {
                                print($">{queueRecords[i].Username}<");
                            }
                            else
                            {
                                print($"{queueRecords[i].Username}");
                            }
                        }
                        break;
                    case "queue.add":
                        await CreateQueue();
                        break;
                    case "queue.delete":
                        await DeleteQueue();
                        break;
                    case "queue.addUser":
                        await AddUserToQueue();
                        break;
                    case "queue.removeRecord":
                        await RemoveUserFromQueue();
                        break;

                    case "queue.moveNext":
                        var current = await SelectQueue();

                        current.MoveToNext();

                        await queueDataSerivce.Update(current);
                        break;
                    case "role.add":
                        await AddRole();
                        break;
                    case "role.view":
                        await ViewRoles();
                        break;
                    case "role.delete":
                        await DeleteRole();
                        break;
                    default:
                        print("Unknown command. type 'help' to help");
                        commandUsed = false;
                        break;

                }

                if (commandUsed)
                    print("Process was ended. Waiting for new commands");
            }
            catch (Exception ex)
            {
                print("Error: " + ex.Message);
            }
        }
    }

    private static async Task DeleteRole()
    {
        var role = await SelectUserRole();

        await userRoleDataService.Delete(role.Id);
    }

    private static async Task ViewRoles()
    {
        var roles = (await userRoleDataService.GetAllRoles()).ToArray();

        foreach (var role in roles)
        {
            print($"> Role Name: {role.Name}; Id: {role.Id}; Can Manage Users: {role.CanManageUsers}; Can Manage Queues: {role.CanManageQueues};");
        }
    }

    private static async Task AddRole()
    {
        string roleName = read("Role Name:");
        bool canManageUsers = read("Can Manage Users? (true/false)").ToLower() == "true";
        bool canManageQueues = read("Role Name Queues? (true/false)").ToLower() == "true";

        await userRoleDataService.Create(roleName, canManageUsers, canManageQueues);
    }

    private static async Task PrintQueuesInfo()
    {
        var infos = (await queueDataSerivce.GetAll()).ToList();

        print($"Queues:");

        for (int i = 0; i < infos.Count; i++)
        {
            var info = infos[i];
            print($"> Id: {info.Id}; Name: {info.Name}; DateTime Created: {info.Created}");
        }
    }

    private static async Task RemoveUserFromQueue()
    {
        var queue = await SelectQueue();

        string username = read("Enter Username:");

        var record = queue.Records.First(x => x.Username == username);

        queue.RemoveRecord(record);

        await queueDataSerivce.Update(queue);
    }

    private static async Task AddUserToQueue()
    {
        var queue = await SelectQueue();

        string username = read("Enter Username:");

        queue.AddUser(username);

        await queueDataSerivce.Update(queue);
    }

    private static async Task DeleteQueue()
    {
        int id = int.Parse(read("Enter queue id:"));

        await queueDataSerivce.Delete(id);
    }

    private static async Task CreateUser()
    {
        print("Enter the values for the required fields:");

        string fullName = read("Full Name:");
        string username = read("Username:");
        string password = read("Password:");

        UserRole role = await SelectUserRole();

        await userDataService.Create(username, fullName, password, role.Id);
    }

    private static async Task<UserRole> SelectUserRole()
    {
        print("Please select User Role (enter Id):");

        var roles = (await userRoleDataService.GetAllRoles()).ToArray();

        foreach (var role in roles)
        {
            print($"> Role Name: {role.Name}; Id: {role.Id}; Can Manage Users: {role.CanManageUsers}; Can Manage Queues: {role.CanManageQueues};");
        }

        int id = int.Parse(read(string.Empty));

        return roles.First(x => x.Id == id);
    }

    private static async Task StartUserPasswordUpdate()
    {
        string username = read("Enter Username:");
        string oldPassword = read("Enter Old Password:");
        string newPassword = read("Enter New Password:");

        User user = await userDataService.Get(username);

        user.UpdatePassword(oldPassword, newPassword);

        await userDataService.Update(user);
    }

    private static async Task StartUserRename()
    {
        string username = read("Enter Username:");
        string fullName = read("Enter New Full Name:");

        User user = await userDataService.Get(username);

        user.FullName = fullName;

        await userDataService.Update(user);
    }

    private static async Task DeleteUser()
    {
        print("Enter the username:");

        string username = read("Username:");

        await userDataService.Delete(username);
    }

    private static async Task CreateQueue()
    {
        print("Enter the values for the required fields:");

        bool flag = read("Should this queue start again after last person in queue? (true/false):") == "true";
        string name = read("Enter Queue Name");

        await queueDataSerivce.Create(name, flag);
    }

    private static async Task<Queue> SelectQueue()
    {
        int id = int.Parse(read("Please Select Queue (int id)"));

        return await queueDataSerivce.Get(id);
    }

    private static string read(string text)
    {
        print(text);
        string? res = Console.ReadLine();

        while (res is null)
        {
            print("Invalid Input. Try Again.");
            res = Console.ReadLine();
        }

        return res;
    }

    private static void print(string text)
    {
        Console.WriteLine(text);
    }
}