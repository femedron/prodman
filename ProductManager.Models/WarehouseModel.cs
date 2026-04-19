using ProductManager.Models.Enums;

namespace ProductManager.Models;

/// <summary>
/// Storage model for a warehouse entity.
/// Responsibility: holds raw persisted data only.
/// Does NOT contain collections of products or computed fields (e.g. total value).
/// </summary>
public class WarehouseModel
{
    public Guid Id { get; }

    public string Name { get; set; }

    public WarehouseLocation Location { get; set; }

    public WarehouseModel(Guid id, string name, WarehouseLocation location)
    {
        Id = id;
        Name = name;
        Location = location;
    }

    public WarehouseModel(string name, WarehouseLocation location)
        : this(Guid.NewGuid(), name, location) { }
}
