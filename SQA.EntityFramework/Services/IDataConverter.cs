using SQA.Domain;
using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services;

public interface IDataConverter<TSource, TTarget>
where TSource : DataTableObject
where TTarget : DomainObject
{
    TTarget Convert(TSource item);

    TSource Convert(TTarget item);
}