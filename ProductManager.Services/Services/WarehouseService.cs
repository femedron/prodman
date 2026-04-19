using ProductManager.Repositories.Repositories;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Dto;

namespace ProductManager.Services.Services;

/// <summary>
/// Реалізація <see cref="IWarehouseService"/>.
/// Отримує дані з репозиторіїв, конвертує їх у DTO і повертає UI-шару.
/// Не знає деталей сховища — взаємодіє з репозиторіями через інтерфейси.
/// </summary>
public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepo;
    private readonly IProductRepository   _productRepo;

    /// <summary>
    /// Залежності передаються через конструктор — Constructor Injection.
    /// IoC-контейнер автоматично надає реалізації під час резолюції.
    /// </summary>
    public WarehouseService(
        IWarehouseRepository warehouseRepo,
        IProductRepository productRepo)
    {
        _warehouseRepo = warehouseRepo;
        _productRepo   = productRepo;
    }

    /// <inheritdoc/>
    public IEnumerable<WarehouseListDto> GetAll()
        => _warehouseRepo.GetAll().Select(w => new WarehouseListDto
        {
            Id           = w.Id,
            Name         = w.Name,
            Location     = w.Location.ToString(),
            ProductCount = _productRepo.GetByWarehouseId(w.Id).Count()
        });

    /// <inheritdoc/>
    public WarehouseDetailDto? GetDetail(Guid id)
    {
        var warehouse = _warehouseRepo.GetById(id);
        if (warehouse is null) return null;

        var products = _productRepo
            .GetByWarehouseId(id)
            .Select(p => new ProductListDto
            {
                Id         = p.Id,
                Name       = p.Name,
                Category   = p.Category.ToString(),
                Quantity   = p.Quantity,
                UnitPrice  = p.UnitPrice,
                TotalValue = p.UnitPrice * p.Quantity
            })
            .ToList();

        return new WarehouseDetailDto
        {
            Id         = warehouse.Id,
            Name       = warehouse.Name,
            Location   = warehouse.Location.ToString(),
            TotalValue = products.Sum(p => p.TotalValue),
            Products   = products
        };
    }
}
