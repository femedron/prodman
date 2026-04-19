using ProductManager.Repositories.DbModels;
using ProductManager.Repositories.Storage;

namespace ProductManager.Repositories.Repositories;

/// <summary>
/// Конкретна реалізація <see cref="IProductRepository"/>.
/// </summary>
public class ProductRepository : IProductRepository
{
    public IEnumerable<ProductDbModel> GetByWarehouseId(Guid warehouseId)
        => FakeStorage.Products.Where(p => p.WarehouseId == warehouseId);

    public ProductDbModel? GetById(Guid id)
        => FakeStorage.Products.FirstOrDefault(p => p.Id == id);
}
