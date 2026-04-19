using ProductManager.Models;
using ProductManager.Models.Enums;

namespace ProductManager.ViewModels;

/// <summary>
/// View/edit model for a product.
/// Responsibility: provides a rich representation suitable for display and editing.
/// Contains the computed TotalValue field (UnitPrice * Quantity).
/// </summary>
public class ProductViewModel
{
    public Guid Id { get; }

    public Guid WarehouseId { get; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public ProductCategory Category { get; set; }

    public string Description { get; set; }

    public decimal TotalValue => UnitPrice * Quantity;

    public ProductViewModel(
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

    public static ProductViewModel FromModel(ProductModel model)
        => new(model.Id, model.WarehouseId, model.Name, model.Quantity,
               model.UnitPrice, model.Category, model.Description);

    /// <summary>Returns a compact one-line summary used in list views.</summary>
    public string ToSummaryString()
        => $"[{Id.ToString()[..8]}] {Name,-35} {Category,-15} {Quantity,6} шт  {UnitPrice,10:N2} грн/шт  Σ {TotalValue,12:N2} грн";

    public string ToDetailString()
        => $"""
            ──────────────────────────────────────────
            Товар:       {Name}
            ID:          {Id}
            Склад ID:    {WarehouseId}
            Категорія:   {Category}
            Кількість:   {Quantity} шт
            Ціна/шт:     {UnitPrice:N2} грн
            Загалом:     {TotalValue:N2} грн
            Опис:        {Description}
            ──────────────────────────────────────────
            """;
}
