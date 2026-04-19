using ProductManager.Services.Dto;

namespace ProductManager.Services.Abstractions;

/// <summary>
/// Абстракція сервісу складів.
/// UI-шар взаємодіє з даними виключно через цей інтерфейс.
/// </summary>
public interface IWarehouseService
{
    /// <summary>Повертає всі склади у форматі для списку.</summary>
    IEnumerable<WarehouseListDto> GetAll();

    /// <summary>Повертає деталі складу разом зі списком його товарів.</summary>
    WarehouseDetailDto? GetDetail(Guid id);
}
