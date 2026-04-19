using ProductManager.Services.Dto;

namespace ProductManager.Services.Abstractions;

/// <summary>
/// Async-сервіс складів. UI взаємодіє виключно через цей інтерфейс.
/// </summary>
public interface IWarehouseService
{
    Task<IEnumerable<WarehouseListDto>>      GetAllAsync();
    Task<WarehouseDetailDto?>                GetDetailAsync(Guid id);
    Task<Guid>                               AddAsync(WarehouseFormDto form);
    Task                                     UpdateAsync(WarehouseFormDto form);
    Task                                     DeleteAsync(Guid id);

    /// <summary>Список рядків enum WarehouseLocation для комбобоксів.</summary>
    IEnumerable<string> GetLocations();
}
