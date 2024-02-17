using SQA.Domain;
using SQA.Domain.Services;
using SQA.EntityFramework.Model;
using SQA.EntityFramework.Services.DataTable;

namespace SQA.EntityFramework.Services.Domain;

public class QueueDataProvider : IQueueDataProvider
{
    private readonly IQueueItemProvider _queueItemProvider;

    private readonly IDataConverter<QueueItem, Queue> _converter;

    public async Task<Queue> Get(int id)
    {
        QueueItem userObject = await _queueItemProvider.Get(id);

        Queue user = _converter.Convert(userObject);

        return user;
    }

    public async Task<IEnumerable<Queue>> GetAll()
    {
        IEnumerable<QueueItem> queueObjects = await _queueItemProvider.GetAll();

        List<Queue> queues = new();

        foreach (QueueItem queueObject in queueObjects)
        {
            Queue queue = _converter.Convert(queueObject);

            queues.Add(queue);
        }

        return queues;
    }

    public QueueDataProvider(IQueueItemProvider queueItemProvider,
                           IDataConverter<QueueItem, Queue> converter)
    {
        _queueItemProvider = queueItemProvider;
        _converter = converter;
    }
}