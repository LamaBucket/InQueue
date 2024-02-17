using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services.DataTable;

public interface IDataProvider<T> where T : DataTableObject
{
    Task<IEnumerable<T>> GetAll();
}