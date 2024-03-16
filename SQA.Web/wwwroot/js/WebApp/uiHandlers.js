_onConnectionStarted = OnConnectionStarted;

_onQueueListLoaded = OnQueueListLoaded;
_onQueueLoaded;

_onQueueCreated = OnQueueCreated;
_onQueueRemoved = OnQueueRemoved;
_onUserRemovedFromQueue = OnQueueRemoved;

_onQueuePositionChanged = OnPositionChanged;


function OnConnectionStarted()
{
    RequestLoadQueues();
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


function OnQueueCreated()
{
    Reload();
}

function OnQueueRemoved()
{
    Reload();
}


function OnPositionChanged(queueId)
{
    Reload();
}
