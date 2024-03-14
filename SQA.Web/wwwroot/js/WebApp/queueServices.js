const hubConnection = new signalR.HubConnectionBuilder()
.withUrl("/queue")
.build();


function RequestCreateQueue(queueName)
{
    hubConnection.invoke("CreateQueue", queueName);
}

hubConnection.start()
.catch(function (err) {
    return console.error(err.toString());
});