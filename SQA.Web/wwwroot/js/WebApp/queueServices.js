var _onConnectionStarted;

var _onQueueListLoaded;

var _onQueueCreated;
var _onQueueRemoved;
var _onUserRemovedFromQueue;

var _onQueuePositionChanged;


const hubConnection = new signalR.HubConnectionBuilder()
.withUrl("/queue")
.build();


function RequestLoadQueues()
{
    hubConnection.invoke("LoadQueueList");
}


function RequestCreateQueue(queueName)
{
    hubConnection.invoke("CreateQueue", queueName);
}

function RequestRemoveQueue(id)
{
    hubConnection.invoke("RemoveQueue", id);
}

function RequestLeaveQueue(id)
{
    hubConnection.invoke("LeaveQueue", id);
}


hubConnection.on("QueueListLoaded", function(queues){
    _onQueueListLoaded(queues);
})

hubConnection.on("QueueCreated", function(){
    _onQueueCreated();
})

hubConnection.on("QueueRemoved", function(id){
    _onQueueRemoved(id);
})

hubConnection.on("RemovedFromQueue", function(id){
    _onUserRemovedFromQueue(id);
})


hubConnection.on("PositionChanged", function(queueId){
    _onQueuePositionChanged(queueId);
})


hubConnection.start().then(function(){
    _onConnectionStarted();
})
.catch(function (err) {
    return console.error(err.toString());
});
