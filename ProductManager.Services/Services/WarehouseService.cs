using ProductManager.Repositories.DbModels;
using ProductManager.Repositories.DbModels.Enums;
using ProductManager.Repositories.Repositories;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Dto;

namespace ProductManager.Services.Services;

/// <summary>
/// Реалізація <see cref="IWarehouseService"/>.
/// Отримує дані з репозиторіїв, конвертує у DTO, виконує CRUD.
/// </summary>
public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepo;
    private readonly IProductRepository   _productRepo;

    public WarehouseService(IWarehouseRepository warehouseRepo, IProductRepository productRepo)
    {
        _warehouseRepo = warehouseRepo;
        _productRepo   = productRepo;
    }

    public async Task<IEnumerable<WarehouseListDto>> GetAllAsync()
    {
        var warehouses = await _warehouseRepo.GetAllAsync();
        var result = new List<WarehouseListDto>();
        foreach (var w in warehouses)
        {
            var products = await _productRepo.GetByWarehouseIdAsync(w.Id);
            result.Add(new WarehouseListDto
            {
                Id           = w.Id,
                Name         = w.Name,
                Location     = w.Location.ToString(),
                ProductCount = products.Count()
            });
        }
        return result;
    }

    public async Task<WarehouseDetailDto?> GetDetailAsync(Guid id)
    {
        var w = await _warehouseRepo.GetByIdAsync(id);
        if (w is null) return null;

        var products = (await _productRepo.GetByWarehouseIdAsync(id))
            .Select(p => new ProductListDto
            {
                Id         = p.Id,
                Name       = p.Name,
                Category   = p.Category.ToString(),
                Quantity   = p.Quantity,
                UnitPrice  = p.UnitPrice,
                TotalValue = p.UnitPrice * p.Quantity
            }).ToList();

        return new WarehouseDetailDto
        {
            Id         = w.Id,
            Name       = w.Name,
            Location   = w.Location.ToString(),
            TotalValue = products.Sum(p => p.TotalValue),
            Products   = products
        };
    }

    public async Task<Guid> AddAsync(WarehouseFormDto form)
    {
        var model = new WarehouseDbModel(form.Name,
            Enum.Parse<WarehouseLocation>(form.Location));
        // Set name via property
        model = new WarehouseDbModel(model.Id, form.Name,
            Enum.Parse<WarehouseLocation>(form.Location));
        return await _warehouseRepo.AddAsync(model);
    }

    public async Task UpdateAsync(WarehouseFormDto form)
    {
        if (form.Id is null) return;
        var model = new WarehouseDbModel(
            form.Id.Value,
            form.Name,
            Enum.Parse<WarehouseLocation>(form.Location));
        await _warehouseRepo.UpdateAsync(model);
    }

    public async Task DeleteAsync(Guid id) =>
        await _warehouseRepo.DeleteAsync(id);

    public IEnumerable<string> GetLocations() =>
        Enum.GetNames<WarehouseLocation>();
}
