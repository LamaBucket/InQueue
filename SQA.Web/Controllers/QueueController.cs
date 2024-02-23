using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQA.Domain.Services.Data;
using SQA.EntityFramework.Services;

namespace SQA.Web.Controllers;

public class QueueController : AuthenticatedController
{
    public readonly IQueueDataService _queueDataService;


    [HttpGet("/user")]
    public async Task<ActionResult> GetAllQueuesForUser()
    {
        var queues = await _queueDataService.GetForUser(_username);

        return Json(queues);
    }

    [HttpPost("/user")]
    public async Task<ActionResult> SignInQueue(int queueId)
    {
        var queue = await _queueDataService.Get(queueId);

        queue.AddUser(_username);

        await _queueDataService.Update(queue);

        return Ok();
    }

    [HttpDelete("/user")]
    public async Task<ActionResult> LeaveQueue(int queueId, string? username = null)
    {
        if (username is null)
            username = _username;
        else
        {
            if (!await CanManageQueue(queueId))
            {
                return Forbid();
            }
        }
        var queue = await _queueDataService.Get(queueId);

        var record = queue.Records.First(x => x.Username == _username);

        queue.RemoveRecord(record);

        await _queueDataService.Update(queue);

        return Ok();
    }

    [HttpPut("/user")]
    public async Task<ActionResult> MoveNext(int queueId)
    {
        if (await CanMoveNext(queueId))
        {
            var queue = await _queueDataService.Get(queueId);

            queue.MoveToNext();

            await _queueDataService.Update(queue);

            return Ok();
        }

        return Forbid();
    }

    [HttpPost]
    public async Task<ActionResult> CreateQueue(string name)
    {
        string owner = _username;

        await _queueDataService.Create(name, owner);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveQueue(int id)
    {
        if (await CanManageQueue(id))
        {
            await _queueDataService.Delete(id);

            return Ok();
        }

        return Forbid();
    }

    [HttpPut]
    public async Task<ActionResult> PassLeadership(int id, string username)
    {
        if (await CanManageQueue(id))
        {
            var queue = await _queueDataService.Get(id);

            queue.QueueInfo.PassLeadership(username);

            await _queueDataService.Update(queue);

            return Ok();
        }

        return Forbid();
    }


    private async Task<bool> CanMoveNext(int queueId)
    {
        if (await CanManageQueue(queueId))
            return true;

        var queue = await _queueDataService.Get(queueId);

        var records = queue.Records.ToArray();

        string currentUsername = records[queue.CurrentPosition].Username;

        return _username == currentUsername;
    }

    private async Task<bool> CanManageQueue(int queueId)
    {
        var user = await GetUser();

        if (user.Role.CanManageQueues)
            return true;

        var queue = await _queueDataService.Get(queueId);

        string username = user.Username;
        string queueOwner = queue.QueueInfo.OwnerUsername;

        return username == queueOwner;
    }


    public QueueController(IQueueDataService queueDataService, IUserDataService userDataService) : base(userDataService)
    {
        _queueDataService = queueDataService;
    }
}