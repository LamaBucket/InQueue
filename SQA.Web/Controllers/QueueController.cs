using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;

public class QueueController : AuthenticatedController
{
    public readonly IQueueDataService _queueDataService;


    [HttpGet]
    public async Task<ActionResult> GetAllQueues()
    {
        var queues = await _queueDataService.GetAll();

        return Json(queues);
    }

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
    public async Task<ActionResult> LeaveQueue(int queueId)
    {
        var queue = await _queueDataService.Get(queueId);

        var record = queue.Records.First(x => x.Username == _username);

        queue.RemoveRecord(record);

        await _queueDataService.Update(queue);

        return Ok();
    }

    [HttpPut("/user")]
    public async Task<ActionResult> MoveNext(int queueId)
    {
        var queue = await _queueDataService.Get(queueId);

        var records = queue.Records.ToArray();


        string currentUsername = records[queue.CurrentPosition].Username;

        if (currentUsername == _username)
        {
            queue.MoveToNext();

            await _queueDataService.Update(queue);

            return Ok();
        }

        return Forbid();
    }

    [HttpPost]
    public async Task<ActionResult> CreateQueue(string name, bool isInfinite)
    {
        if (await CanManageQueue())
        {
            await _queueDataService.Create(name, isInfinite);

            return Ok();
        }

        return Forbid();
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveQueue(int id)
    {
        if (await CanManageQueue())
        {
            await _queueDataService.Delete(id);

            return Ok();
        }

        return Forbid();
    }


    private async Task<bool> CanManageQueue()
    {
        var role = await GetUserRole();

        return role.CanManageQueues;
    }


    public QueueController(IQueueDataService queueDataService, IUserDataService userDataService) : base(userDataService)
    {
        _queueDataService = queueDataService;
    }
}