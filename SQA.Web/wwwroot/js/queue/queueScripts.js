$(document).ready(function() {
   $(".btnMoveToNext").click(function(){
    RequestMoveNext();
   })
});

_onConnectionStarted = OnConnectionStarted;

_onQueueLoaded = OnQueueLoaded;

_onQueueRemoved = RedirectToWebApp;
_onUserRemovedFromQueue = RedirectToWebApp;

_onMoveNext = OnMoveNext;
_onQueueRecordChanged = OnQueueRecordChanged;

var CurrentQueuePosition = -1;


function OnConnectionStarted(){
    RequestLoadQueue(QueueId);
}

function OnQueueLoaded(queue){
    CurrentQueuePosition = queue.currentPosition;
    ParseRecords(queue.records);
}

function ParseRecords(records){

    var currentUsername = records[CurrentQueuePosition].username;
    
    SetCurrentUser(currentUsername);
    ClearUserList();

    records = EnsureRecordsInRightOrder(records);

    var list_position = 0;

    for(i = CurrentQueuePosition + 1; i < records.length; i++){
        var record = records[i];
        AppendUser(record.username, list_position);
    }
    
    for(i = 0; i < CurrentQueuePosition; i++){
        var record = records[i];
        AppendUser(record.username, list_position);
    }
}

function SetCurrentUser(username){
    $(".label-current-user").text(username);
}

function EnsureRecordsInRightOrder(records){
    result = []
    
    for(var i = 0; i < records.length; i++){
        var record = records[i];

        var position = record.position;

        result[position] = record;
    }

    return result;
}

function RedirectToWebApp(){
    location.href = "/Web";
}

function OnQueueRecordChanged(){
    RequestLoadQueue(QueueId);
}

function OnMoveNext(){
    RequestLoadQueue();
}



function AppendUser(username, position)
{
    var html = `<li class="user-item ${username}"></li>`;

    var itemsCount = $("ul.user-list li").length;

    if(itemsCount == 0 | itemsCount <= position)
    {
        $(`.user-list`).append(html);
    }
    else
    {
        $(`.user-list li:eq(${position})`).before(html);
    }
    
    $("." + username).append(`<span class="user-name">${username}</span>`);
    
    if(CanManageQueue)
    {
        $("." + username).append(`<button class="x-button btnDeleteUser">X</button>`)
    }
}

function RemoveUser(username)
{
    $("." + username).remove();
}

function ClearUserList()
{
    $(".user-list").empty();
}
