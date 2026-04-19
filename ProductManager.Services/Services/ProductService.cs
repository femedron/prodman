using ProductManager.Repositories.DbModels;
using ProductManager.Repositories.DbModels.Enums;
using ProductManager.Repositories.Repositories;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Dto;

namespace ProductManager.Services.Services;

/// <summary>Реалізація <see cref="IProductService"/>.</summary>
public class ProductService : IProductService
{
    private readonly IProductRepository   _productRepo;
    private readonly IWarehouseRepository _warehouseRepo;

    public ProductService(IProductRepository productRepo, IWarehouseRepository warehouseRepo)
    {
        _productRepo   = productRepo;
        _warehouseRepo = warehouseRepo;
    }

    public async Task<ProductDetailDto?> GetDetailAsync(Guid id)
    {
        var p = await _productRepo.GetByIdAsync(id);
        if (p is null) return null;
        var wName = (await _warehouseRepo.GetByIdAsync(p.WarehouseId))?.Name ?? string.Empty;
        return new ProductDetailDto
        {
            Id            = p.Id,
            WarehouseId   = p.WarehouseId,
            WarehouseName = wName,
            Name          = p.Name,
            Category      = p.Category.ToString(),
            Quantity      = p.Quantity,
            UnitPrice     = p.UnitPrice,
            TotalValue    = p.UnitPrice * p.Quantity,
            Description   = p.Description
        };
    }

    public async Task<Guid> AddAsync(ProductFormDto form)
    {
        var model = new ProductDbModel(
            form.WarehouseId,
            form.Name,
            form.Quantity,
            form.UnitPrice,
            Enum.Parse<ProductCategory>(form.Category),
            form.Description);
        return await _productRepo.AddAsync(model);
    }

    public async Task UpdateAsync(ProductFormDto form)
    {
        if (form.Id is null) return;
        var model = new ProductDbModel(
            form.Id.Value,
            form.WarehouseId,
            form.Name,
            form.Quantity,
            form.UnitPrice,
            Enum.Parse<ProductCategory>(form.Category),
            form.Description);
        await _productRepo.UpdateAsync(model);
    }

    public async Task DeleteAsync(Guid id) =>
        await _productRepo.DeleteAsync(id);

    public IEnumerable<string> GetCategories() =>
        Enum.GetNames<ProductCategory>();
}
