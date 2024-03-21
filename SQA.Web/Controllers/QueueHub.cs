using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SQA.Domain;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;

[Authorize]
public class QueueHub : AuthenticatedHub
{
    private readonly IQueueDataService _queueDataService;
    
    private readonly Dictionary<string, List<string>> _connections;

    public override async Task OnConnectedAsync()
    {
        SaveConnectionId(Context.ConnectionId);

        await BindUserToGroups();

        await base.OnConnectedAsync();
    }

    private async Task BindUserToGroups()
    {
        var infos = await _queueDataService.GetForUser(_username);

        var ids = infos.Select(x => x.Id);

        foreach (var id in ids)
        {
            await StartSendingUpdates(id);
        }
    }


    public async Task LoadQueueList()
    {
        var infos = await _queueDataService.GetForUser(_username);

        await NotifyUserQueueListLoaded(infos);
    }

    private async Task NotifyUserQueueListLoaded(IEnumerable<UserQueueInfo> infos)
    {
        await Clients.Caller.SendAsync("QueueListLoaded", infos);
    }


    public async Task LoadQueue(int queueId)
    {
        var queue = await _queueDataService.Get(queueId);

        await NotifyUserQueueLoaded(queue);
    }

    private async Task NotifyUserQueueLoaded(Queue queue)
    {
        await Clients.Caller.SendAsync("QueueLoaded", queue);
    }


    public async Task MoveNext(int queueId)
    {
        if (await CanMoveNext(queueId))
        {
            var queue = await _queueDataService.Get(queueId);

            queue.MoveToNext();

            await _queueDataService.Update(queue);
        }

        await NotifyUsersPositionChanged(queueId);
    }

    private async Task NotifyUsersPositionChanged(int queueId)
    {
        await GetClientsByQueueId(queueId).SendAsync("PositionChanged", queueId);
    }


    public async Task RemoveFromQueue(int queueId, string username)
    {
        if (!await CanManageQueue(queueId))
        {
            return;
        }

        var queue = await _queueDataService.Get(queueId);

        var record = queue.Records.First(x => x.Username == username);

        queue.RemoveRecord(record);

        await _queueDataService.Update(queue);

        await NotifyUserRemovedFromQueue(queueId, username);
        await NotifyUsersQueueRecordChanged(queueId, queue.Records);
    }

    public async Task LeaveQueue(int queueId)
    {
        var queue = await _queueDataService.Get(queueId);

        var record = queue.Records.First(x => x.Username == _username);

        queue.RemoveRecord(record);

        await _queueDataService.Update(queue);

        await NotifyUserRemovedFromQueue(queueId, _username);
        await NotifyUsersQueueRecordChanged(queueId, queue.Records);
    }

    public async Task SignInQueue(int queueId)
    {
        var queue = await _queueDataService.Get(queueId);

        queue.AddUser(_username);

        await _queueDataService.Update(queue);


        await NotifyUserAddedToQueue(queueId);
        await NotifyUsersQueueRecordChanged(queueId, queue.Records);
    }

    private async Task NotifyUserRemovedFromQueue(int queueId, string username)
    {
        await GetClientByUsername(username).SendAsync("RemovedFromQueue", queueId);
        await StopSendingUpdates(queueId, username);
    }

    private async Task NotifyUserAddedToQueue(int queueId)
    {
        await GetClientByUsername(_username).SendAsync("AddedToQueue", queueId);
        await StartSendingUpdates(queueId);
    }

    private async Task NotifyUsersQueueRecordChanged(int queueId, IEnumerable<QueueRecord> records)
    {
        await GetClientsByQueueId(queueId).SendAsync("QueueRecordChanged", records);
    }


    public async Task CreateQueue(string name)
    {
        string owner = _username;

        await _queueDataService.Create(name, owner);

        await NotifyUserQueueCreated();
    }

    private async Task NotifyUserQueueCreated()
    {
        await Clients.Caller.SendAsync("QueueCreated");
    }


    public async Task RemoveQueue(int queueId)
    {
        if (await CanManageQueue(queueId))
        {
            await _queueDataService.Delete(queueId);
        }

        await NotifyUsersQueueRemoved(queueId);
    }

    private async Task NotifyUsersQueueRemoved(int queueId)
    {
        await GetClientsByQueueId(queueId).SendAsync("QueueRemoved", queueId);
        await StartSendingUpdates(queueId);
    }


    public async Task PassLeadership(int id, string username)
    {
        if (await CanManageQueue(id))
        {
            string oldOwnerUsername;

            var queue = await _queueDataService.Get(id);

            oldOwnerUsername = queue.QueueInfo.OwnerUsername;
            queue.QueueInfo.PassLeadership(username);

            await _queueDataService.Update(queue);

            await NotifyUserRemovedOwner(id, oldOwnerUsername);
            await NotifyUserAddedOwner(id, username);
        }
    }

    private async Task NotifyUserRemovedOwner(int queueId, string oldOwnerUsername)
    {
        await GetClientByUsername(oldOwnerUsername).SendAsync("RemoveOwner", queueId);
    }

    private async Task NotifyUserAddedOwner(int queueId, string username)
    {
        await GetClientByUsername(username).SendAsync("AddOwner", queueId);
    }


    protected async Task StartSendingUpdates(int queueId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, ConvertQueueIdToGroupId(queueId));
    }

    protected async Task StopSendingUpdates(int queueId, string? username = null)
    {
        if (username == null)
        {
            username = _username;
        }

        var connectionIds = GetConnectionIdsForUser(username);

        foreach(var connectionId in connectionIds)
        {
            await Groups.RemoveFromGroupAsync(connectionId, ConvertQueueIdToGroupId(queueId));
        }
    }


    private IClientProxy GetClientsByQueueId(int queueId)
    {
        string groupName = ConvertQueueIdToGroupId(queueId);

        return Clients.Group(groupName);
    }

    private IClientProxy GetClientByUsername(string username)
    {
        return Clients.User(username);
    }

    private string ConvertQueueIdToGroupId(int queueId)
    {
        return queueId.ToString();
    }


    protected void SaveConnectionId(string connectionId)
    {
        if(!_connections.ContainsKey(_username))
        {
            _connections.Add(_username, new());
        }

        _connections[_username].Add(connectionId);
    }

    protected IEnumerable<string> GetConnectionIdsForUser(string username)
    {
        return _connections[username];
    }


    protected async Task<bool> CanMoveNext(int queueId)
    {
        if (await CanManageQueue(queueId))
            return true;

        var queue = await _queueDataService.Get(queueId);

        var records = queue.Records.ToArray();

        string currentUsername = records[queue.CurrentPosition].Username;

        return _username == currentUsername;
    }

    protected async Task<bool> CanManageQueue(int queueId)
    {
        var user = await GetUser();

        if (user.Role.CanManageQueues)
            return true;

        var queue = await _queueDataService.Get(queueId);

        string username = user.Username;
        string queueOwner = queue.QueueInfo.OwnerUsername;

        return username == queueOwner;
    }


    public QueueHub(IQueueDataService queueDataService, IUserDataService userDataService) : base(userDataService)
    {
        _queueDataService = queueDataService;
        _connections = new();
    }
}