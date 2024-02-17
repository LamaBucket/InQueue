using SQA.Domain;

namespace SQA.Domain.Services;


public interface IDataProvider<T> where T : DomainObject
{
    Task<IEnumerable<T>> GetAll();
}