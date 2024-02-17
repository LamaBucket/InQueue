using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services.DataTable;

public interface IDataItemEditService<T> where T : DataTableObject
{
    Task Create(T entity);

    Task Update(T entity);

    Task Delete(T entity);
}