_onQueueListLoaded = OnQueueListLoaded;
_onQueuePositionChanged = OnPositionChanged;
_onQueueCreated = OnQueueCreated;
_onConnectionStarted = OnConnectionStarted;

$(".btnAddQueue").click(function(){ CreateQueue() });

function CreateQueue()
{
    queueName = prompt("Enter Queue Name:");

    RequestCreateQueue(queueName);
}



function OnQueueListLoaded(queues)
{
    for(var i in queues)
    {
        var queue = queues[i];
        var position = queue.userPosition;
        var id = queue.id;
        var name = queue.name;
        var owner = queue.ownerUsername;
        
        AppendQueueToList(id, position, name, true);
    }
}

function AppendQueueToList(id, currentPosition, name, canManage)
{
    var html_id = "queue_" + id;
    var html = `<div class="lst_item queue ${html_id}"></div>`;
    $(".queues").append(html);
    $("." + html_id).append(`<div class="queueName">${name}</div>`)
    $("." + html_id).append(`<div class="queuePosition">${currentPosition}</div>`)
    $("." + html_id).append(`<div class="lbl queuePositionLabel">Position</div>`)
    $("." + html_id).append(`<button class="btnLeaveQueue">X</button>`)
}


function OnPositionChanged(queueId)
{
    Reload();
}

function OnQueueCreated()
{
    Reload();
}

function OnConnectionStarted()
{
    RequestLoadQueues();
}
