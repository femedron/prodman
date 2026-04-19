using ProductManager.Models;
using ProductManager.Services.Abstractions;
using ProductManager.ViewModels;

namespace ProductManager.Services;

/// <summary>
/// Concrete implementation of <see cref="IWarehouseRepository"/> backed by
/// the in-memory <see cref="FakeDataStore"/>.
/// 
/// Registered in the DI container as the implementation of IWarehouseRepository.
/// In later labs this can be swapped for a database-backed implementation
/// without touching any consumer code (Open/Closed Principle).
/// </summary>
public class WarehouseRepository : IWarehouseRepository
{
    // ── Warehouse operations ──────────────────────────────────────────────────

    /// <summary>
    /// Returns all warehouses as view-models (without loading their products).
    /// Products are loaded lazily via <see cref="LoadProductsForWarehouse"/>.
    /// </summary>
    public List<WarehouseViewModel> GetAllWarehouses()
        => FakeDataStore.Warehouses
            .Select(WarehouseViewModel.FromModel)
            .ToList();

    /// <summary>
    /// Returns a single warehouse view-model by its ID, or null if not found.
    /// </summary>
    public WarehouseViewModel? GetWarehouseById(Guid id)
    {
        var model = FakeDataStore.Warehouses.FirstOrDefault(w => w.Id == id);
        return model is null ? null : WarehouseViewModel.FromModel(model);
    }

    // ── Product operations ────────────────────────────────────────────────────

    /// <summary>
    /// Loads products that belong to the given warehouse and attaches them
    /// to the <paramref name="warehouse"/> view-model's Products collection.
    /// Sets <see cref="WarehouseViewModel.ProductsLoaded"/> = true when done.
    /// 
    /// Calling this method a second time on the same view-model replaces
    /// the existing product list (acts as a refresh).
    /// </summary>
    public void LoadProductsForWarehouse(WarehouseViewModel warehouse)
    {
        warehouse.Products = FakeDataStore.Products
            .Where(p => p.WarehouseId == warehouse.Id)
            .Select(ProductViewModel.FromModel)
            .ToList();

        warehouse.ProductsLoaded = true;
    }

    /// <summary>
    /// Returns all products that belong to the given warehouse as view-models.
    /// Does NOT attach them to a WarehouseViewModel.
    /// </summary>
    public List<ProductViewModel> GetProductsByWarehouse(Guid warehouseId)
        => FakeDataStore.Products
            .Where(p => p.WarehouseId == warehouseId)
            .Select(ProductViewModel.FromModel)
            .ToList();

    /// <summary>
    /// Returns a single product view-model by its ID, or null if not found.
    /// </summary>
    public ProductViewModel? GetProductById(Guid id)
    {
        var model = FakeDataStore.Products.FirstOrDefault(p => p.Id == id);
        return model is null ? null : ProductViewModel.FromModel(model);
    }
}
