using Microsoft.AspNetCore.Mvc;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;

public class QueueController : Controller
{
    private string _username
    {
        get
        {
            return ""; //TODO: Get username from cookies;
        }
    }

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

    [HttpPost]
    public async Task<ActionResult> SignInQueue(int queueId)
    {
        var queue = await _queueDataService.Get(queueId);

        queue.AddUser(_username);

        await _queueDataService.Update(queue);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> LeaveQueue(int queueId)
    {
        var queue = await _queueDataService.Get(queueId);

        var record = queue.Records.First(x => x.Username == _username);

        queue.RemoveRecord(record);

        await _queueDataService.Update(queue);

        return Ok();
    }

    [HttpPut]
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

        return BadRequest();
    }


    public QueueController(IQueueDataService queueDataService)
    {
        _queueDataService = queueDataService;
    }
}