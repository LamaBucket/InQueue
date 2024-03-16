var LEAVEQUEUE_QueueId;



function AppendQueueToList(id, currentPosition, name, canManage)
{
    var html_id = "queue_" + id;
    var html = `<div class="lst_item queue ${html_id}" onclick="StartLeaveQueueProcess(${id})"></div>`;
    $(".queues").append(html);
    $("." + html_id).append(`<div class="queueName">${name}</div>`)
    $("." + html_id).append(`<div class="queuePosition">${currentPosition}</div>`)
    $("." + html_id).append(`<div class="lbl queuePositionLabel">Position</div>`)
    $("." + html_id).append(`<button class="btnLeaveQueue">X</button>`)
}

function RemoveQueueFromList(id)
{
    var html_id = "queue_" + id;

    $("." + html_id).remove();
}

function UpdateQueuePosition(id, position)
{
    var html_id = "queue_" + id;

    $("." + html_id).find(".queuePosition").text(position);
}


function StartLeaveQueueProcess(id)
{
    LEAVEQUEUE_QueueId = id;

    ShowConfirmLeaveQueueDialog();
}


function CreateQueue()
{
    var queueName = GetQueueName();
    RequestCreateQueue(queueName);
}

function GetQueueName()
{
    return $(".dialog-queue-name").val();
}

function LeaveQueue()
{
    RequestLeaveQueue(LEAVEQUEUE_QueueId);
}

