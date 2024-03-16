using SQA.Domain.Exceptions;

namespace SQA.Domain;

public class Queue : DomainObject
{
    public readonly QueueInfo QueueInfo;

    public IEnumerable<QueueRecord> Records => _records;

    public int CurrentPosition => _currentPosition;


    private List<QueueRecord> _records;

    private int _currentPosition;

    public void MoveToNext()
    {
        if (_records.Count == 0)
            throw new EmptyQueueException(QueueInfo.Id);

        bool isLast = CurrentPosition == _records.Count - 1;

        if (isLast)
            _currentPosition = 0;
        else
            _currentPosition += 1;

    }

    public void AddUser(string username)
    {
        if (ContainsRecord(username))
            throw new UserAlreadyExistsInQueueException(username, QueueInfo.Id);

        int numberOfUsers = _records.Count;

        QueueRecord record = new(numberOfUsers, username);

        _records.Add(record);
    }

    private bool ContainsRecord(string username)
    {
        return _records.Any(x => x.Username == username);
    }


    public void RemoveRecord(QueueRecord record)
    {
        if (!ContainsRecord(record))
            throw new RecordDoesNotExist(QueueInfo.Id, record);

        _currentPosition -= 1;

        _records.Remove(record);

        for (int i = record.Position + 1; i < _records.Count; i++)
        {
            var nextRecord = _records.First(x => x.Position == i);
            nextRecord.Position = i - 1;
        }

        if(_records.Count == 0)
            return;

        MoveToNext();
    }

    private bool ContainsRecord(QueueRecord record)
    {
        return _records.Contains(record);
    }

    internal Queue(QueueInfo queueInfo, int currentPosition, IEnumerable<QueueRecord> queueRecords)
    {
        QueueInfo = queueInfo;
        _records = new(queueRecords);
        _currentPosition = currentPosition;
        _currentPosition = currentPosition;
    }
}