using ProductManager.Repositories.DbModels;

namespace ProductManager.Repositories.Repositories;

/// <summary>
/// Абстракція репозиторію товарів.
/// </summary>
public interface IProductRepository
{
    Task<IEnumerable<ProductDbModel>> GetByWarehouseIdAsync(Guid warehouseId);
    Task<ProductDbModel?> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(ProductDbModel product);
    Task UpdateAsync(ProductDbModel product);
    Task DeleteAsync(Guid id);
}
