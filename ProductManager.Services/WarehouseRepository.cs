using ProductManager.Models;
using ProductManager.ViewModels;

namespace ProductManager.Services;

/// <summary>
/// Service responsible for all interactions with the data store.
/// </summary>
public class WarehouseRepository
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

    public List<ProductViewModel> GetProductsByWarehouse(Guid warehouseId)
        => FakeDataStore.Products
            .Where(p => p.WarehouseId == warehouseId)
            .Select(ProductViewModel.FromModel)
            .ToList();

    public ProductViewModel? GetProductById(Guid id)
    {
        var model = FakeDataStore.Products.FirstOrDefault(p => p.Id == id);
        return model is null ? null : ProductViewModel.FromModel(model);
    }
}
