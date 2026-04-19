using ProductManager.Repositories.DbModels.Enums;

namespace ProductManager.Repositories.DbModels;

/// <summary>
/// DB-модель товару — зберігає сирі дані так, як вони лежать у сховищі.
/// Посилається на склад лише через WarehouseId (без навігаційного об'єкта).
/// Не містить обчислюваних полів (TotalValue вираховується на сервісному рівні).
/// </summary>
public class ProductDbModel
{
    /// <summary>Унікальний ідентифікатор. Не змінюється після створення.</summary>
    public Guid Id { get; }

    /// <summary>FK на склад. Не змінюється після створення.</summary>
    public Guid WarehouseId { get; }

    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public ProductCategory Category { get; set; }
    public string Description { get; set; }

    public ProductDbModel(
        Guid id, Guid warehouseId, string name,
        int quantity, decimal unitPrice,
        ProductCategory category, string description)
    {
        Id          = id;
        WarehouseId = warehouseId;
        Name        = name;
        Quantity    = quantity;
        UnitPrice   = unitPrice;
        Category    = category;
        Description = description;
    }

    /// <summary>Зручний конструктор з автогенерацією Guid.</summary>
    public ProductDbModel(
        Guid warehouseId, string name,
        int quantity, decimal unitPrice,
        ProductCategory category, string description)
        : this(Guid.NewGuid(), warehouseId, name, quantity, unitPrice, category, description) { }
}
