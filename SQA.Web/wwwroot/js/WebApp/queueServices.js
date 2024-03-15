var _onQueueListLoaded;
var _onQueuePositionChanged;
var _onQueueCreated;
var _onConnectionStarted;

const hubConnection = new signalR.HubConnectionBuilder()
.withUrl("/queue")
.build();


function RequestCreateQueue(queueName)
{
    hubConnection.invoke("CreateQueue", queueName);
}

function RequestLoadQueues()
{
    hubConnection.invoke("LoadQueueList");
}


hubConnection.on("QueueListLoaded", function(queues){
    _onQueueListLoaded(queues);
})

hubConnection.on("PositionChanged", function(queueId){
    _onQueuePositionChanged(queueId);
})

hubConnection.on("QueueCreated", function(){
    _onQueueCreated();
})


hubConnection.start().then(function(){
    _onConnectionStarted();
})
.catch(function (err) {
    return console.error(err.toString());
});
