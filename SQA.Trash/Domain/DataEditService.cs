using SQA.Domain;
using SQA.Domain.Services;
using SQA.EntityFramework.Model;
using SQA.EntityFramework.Services.DataTable;

namespace SQA.EntityFramework.Services.Domain;

public class DataEditService<T, TDataTableObject> : IDataEditService<T>
where T : DomainObject
where TDataTableObject : DataTableObject
{
    private readonly IDataItemEditService<TDataTableObject> _dataItemEditService;

    private readonly IDataConverter<TDataTableObject, T> _converter;

    public async Task Create(T entity)
    {
        TDataTableObject dto = CreateDTO(entity);

        await _dataItemEditService.Create(dto);
    }

    public async Task Delete(T entity)
    {
        TDataTableObject dto = CreateDTO(entity);

        await _dataItemEditService.Delete(dto);
    }

    public async Task Update(T entity)
    {
        TDataTableObject dto = CreateDTO(entity);

        await _dataItemEditService.Update(dto);
    }


    private TDataTableObject CreateDTO(T entity)
    {
        return _converter.Convert(entity);
    }

    public DataEditService(IDataItemEditService<TDataTableObject> dataItemEditService,
                           IDataConverter<TDataTableObject, T> converter)
    {
        _dataItemEditService = dataItemEditService;
        _converter = converter;
    }
}