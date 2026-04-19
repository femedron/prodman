using ProductManager.Repositories.DbModels;

namespace ProductManager.Repositories.Repositories;

/// <summary>
/// Абстракція для доступу до даних товарів.
/// </summary>
public interface IProductRepository
{
    /// <summary>Повертає всі товари певного складу.</summary>
    IEnumerable<ProductDbModel> GetByWarehouseId(Guid warehouseId);

    /// <summary>Повертає товар за ID або null.</summary>
    ProductDbModel? GetById(Guid id);
}
