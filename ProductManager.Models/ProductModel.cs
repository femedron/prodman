using ProductManager.Models.Enums;

namespace ProductManager.Models;

/// <summary>
/// Storage model for a product entity.
/// Responsibility: holds raw persisted data only.
/// References its parent warehouse by ID — does NOT hold a WarehouseModel reference.
/// Does NOT contain computed fields (e.g. TotalValue = Quantity * UnitPrice).
/// </summary>
public class ProductModel
{
    public Guid Id { get; }

    public Guid WarehouseId { get; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public ProductCategory Category { get; set; }

    public string Description { get; set; }

    public ProductModel(
        Guid id,
        Guid warehouseId,
        string name,
        int quantity,
        decimal unitPrice,
        ProductCategory category,
        string description)
    {
        Id = id;
        WarehouseId = warehouseId;
        Name = name;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Category = category;
        Description = description;
    }

    public ProductModel(
        Guid warehouseId,
        string name,
        int quantity,
        decimal unitPrice,
        ProductCategory category,
        string description)
        : this(Guid.NewGuid(), warehouseId, name, quantity, unitPrice, category, description) { }
}
