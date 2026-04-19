using ProductManager.Services.Dto;

namespace ProductManager.Services.Abstractions;

/// <summary>
/// Абстракція сервісу товарів.
/// UI-шар взаємодіє з товарами виключно через цей інтерфейс.
/// </summary>
public interface IProductService
{
    /// <summary>Повертає деталі конкретного товару або null.</summary>
    ProductDetailDto? GetDetail(Guid id);
}
