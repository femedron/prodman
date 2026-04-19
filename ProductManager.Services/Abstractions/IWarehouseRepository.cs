using ProductManager.ViewModels;

namespace ProductManager.Services.Abstractions;

/// <summary>
/// Abstraction for warehouse data access.
/// All consumers (UI, console, tests) depend on this interface,
/// never on the concrete implementation — Dependency Inversion Principle.
/// </summary>
public interface IWarehouseRepository
{
    /// <summary>Returns all warehouses as view-models (products NOT loaded).</summary>
    List<WarehouseViewModel> GetAllWarehouses();

    /// <summary>Returns a single warehouse by ID, or null.</summary>
    WarehouseViewModel? GetWarehouseById(Guid id);

    /// <summary>
    /// Loads products for the given warehouse view-model and sets ProductsLoaded = true.
    /// </summary>
    void LoadProductsForWarehouse(WarehouseViewModel warehouse);

    /// <summary>Returns products of a warehouse directly, without attaching to a view-model.</summary>
    List<ProductViewModel> GetProductsByWarehouse(Guid warehouseId);

    /// <summary>Returns a single product by ID, or null.</summary>
    ProductViewModel? GetProductById(Guid id);
}
