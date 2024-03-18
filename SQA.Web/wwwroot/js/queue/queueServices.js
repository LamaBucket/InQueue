var _onConnectionStarted;

var _onQueueLoaded;

var _onQueueRemoved;
var _onUserRemovedFromQueue;

var _onMoveNext;
var _onQueueRecordChanged;


const hubConnection = new signalR.HubConnectionBuilder()
.withUrl("/queue")
.build();


function RequestLoadQueue()
{
    hubConnection.invoke("LoadQueue", QueueId);
}

function RequestMoveNext()
{
    hubConnection.invoke("MoveNext", QueueId);
}


hubConnection.on("QueueLoaded", function(queue){
    _onQueueLoaded(queue);
})

hubConnection.on("QueueRemoved", function(id){
    if(IsForCurrentQueue(id)){
        _onQueueRemoved();
    }
})

hubConnection.on("RemovedFromQueue", function(id){
    if(IsForCurrentQueue(id)){
        _onUserRemovedFromQueue();
    }
})


hubConnection.on("PositionChanged", function(queueId){
    if(IsForCurrentQueue(queueId)){
        _onMoveNext();
    }
})

hubConnection.on("QueueRecordChanged", function(id,  records){
    if(IsForCurrentQueue(id)){
        _onQueueRecordChanged();
    }
})

hubConnection.on("RemovedOwner", function(queueId){
    if(IsForCurrentQueue(queueId)){
        Refresh();
    }
})

hubConnection.on("AddedOwner", function(queueId){
    if(IsForCurrentQueue(queueId)){
        Refresh();
    }
})

function IsForCurrentQueue(changedQueueId)
{
    return changedQueueId == QueueId;
}

hubConnection.start().then(function(){
    _onConnectionStarted();
})
.catch(function (err) {
    return console.error(err.toString());
});
