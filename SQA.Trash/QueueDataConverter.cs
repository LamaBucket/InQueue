using SQA.Domain;
using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services;

public class QueueDataConverter : IDataConverter<QueueItem, Queue>
{
    public Queue Convert(QueueItem item)
    {
        if (item.Records is null)
            throw new Exception();



        item.Records.Select(x => new QueueRecord(x.Position, x.Username));

        QueueInfo info = new(item.QueueId, item.QueueName, DateTime.Now);

        return new Queue(info, item.IsInfinite, );
    }

    public QueueItem Convert(Queue item)
    {
        return new QueueItem(item.Id, item.Name, item.IsInfinite, item.CurrentPosition);
    }
}
