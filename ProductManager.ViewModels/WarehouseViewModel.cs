using ProductManager.Models;
using ProductManager.Models.Enums;

namespace ProductManager.ViewModels;

/// <summary>
/// View/edit model for a warehouse.
/// Responsibility: provides a rich representation suitable for display and editing.
/// Contains computed fields and a collection of associated product view-models.
/// Built from a WarehouseModel + its products by the service layer.
/// </summary>
public class WarehouseViewModel
{
    public Guid Id { get; }

    public string Name { get; set; }

    public WarehouseLocation Location { get; set; }

    public List<ProductViewModel> Products { get; set; } = new();

    public decimal TotalValue => Products.Sum(p => p.TotalValue);

    public bool ProductsLoaded { get; set; } = false;

    public WarehouseViewModel(Guid id, string name, WarehouseLocation location)
    {
        Id = id;
        Name = name;
        Location = location;
    }

    public static WarehouseViewModel FromModel(WarehouseModel model)
        => new(model.Id, model.Name, model.Location);

    /// <summary>Returns a one-line summary used in list views.</summary>
    public string ToSummaryString()
        => $"[{Id.ToString()[..8]}] {Name} — {Location}";

    public string ToDetailString()
    {
        var loaded = ProductsLoaded
            ? $"Товарів: {Products.Count}  |  Загальна вартість: {TotalValue:N2} грн"
            : "(товари не завантажено)";

        return $"""
                ══════════════════════════════════════════
                Склад:       {Name}
                ID:          {Id}
                Локація:     {Location}
                {loaded}
                ══════════════════════════════════════════
                """;
    }
}
