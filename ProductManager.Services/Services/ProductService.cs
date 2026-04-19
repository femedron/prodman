using ProductManager.Repositories.Repositories;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Dto;

namespace ProductManager.Services.Services;

/// <summary>
/// Реалізація <see cref="IProductService"/>.
/// Конвертує <see cref="Repositories.DbModels.ProductDbModel"/> у
/// <see cref="ProductDetailDto"/> для UI-шару.
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository   _productRepo;
    private readonly IWarehouseRepository _warehouseRepo;

    public ProductService(
        IProductRepository productRepo,
        IWarehouseRepository warehouseRepo)
    {
        _productRepo   = productRepo;
        _warehouseRepo = warehouseRepo;
    }

    /// <inheritdoc/>
    public ProductDetailDto? GetDetail(Guid id)
    {
        var product = _productRepo.GetById(id);
        if (product is null) return null;

        // Назва складу підтягується тут, щоб UI не звертався до WarehouseRepository.
        var warehouseName = _warehouseRepo.GetById(product.WarehouseId)?.Name ?? string.Empty;

        return new ProductDetailDto
        {
            Id            = product.Id,
            WarehouseId   = product.WarehouseId,
            WarehouseName = warehouseName,
            Name          = product.Name,
            Category      = product.Category.ToString(),
            Quantity      = product.Quantity,
            UnitPrice     = product.UnitPrice,
            TotalValue    = product.UnitPrice * product.Quantity,
            Description   = product.Description
        };
    }
}
