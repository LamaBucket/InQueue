var LEAVEQUEUE_QueueId;

$(".btnJoinQueue").on('click', function(){
    var queueId = prompt("Enter Queue Id:");

    RequestSignInQueue(queueId);
})


function AppendQueueToList(id, currentPosition, name, canManage)
{
    var html_id = "queue_" + id;
    var html = `<div class="lst_item queue ${html_id}"></div>`;
    $(".queues").append(html);
    $("." + html_id).append(`<div class="queueName">${name}</div>`)
    $("." + html_id).append(`<div class="queuePosition">${currentPosition}</div>`)
    $("." + html_id).append(`<div class="lbl queuePositionLabel">Position</div>`)

    $("." + html_id).on('click', function(){
        OpenQueue(id);
    })

    if(canManage){
        $("." + html_id).append(`<button class="btnLeaveQueue leave_${html_id}">X</button>`)
        $(".leave_" + html_id).on('click', function(e){
            e.stopPropagation();
            StartLeaveQueueProcess(id);
        })
    }
}

function RemoveQueueFromList(id)
{
    var html_id = "queue_" + id;

    $("." + html_id).remove();
}

function EmptyQueueList(){
    $(".queues").empty();
}


function StartLeaveQueueProcess(id)
{
    LEAVEQUEUE_QueueId = id;

    ShowConfirmLeaveQueueDialog();
}

function OpenQueue(id){
    return location.href = "/Queue/" + id; 
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
