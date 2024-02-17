namespace SQA.Domain.Services;

public interface IDataEditService<T> where T : DomainObject
{
    Task Create(T entity);

    Task Update(T entity);

    Task Delete(T entity);
}