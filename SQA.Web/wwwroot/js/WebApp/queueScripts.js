$(".btnAddQueue").click(function(){ CreateQueue() });


function CreateQueue()
{
    queueName = prompt("Enter Queue Name:");

    RequestCreateQueue(queueName);
}