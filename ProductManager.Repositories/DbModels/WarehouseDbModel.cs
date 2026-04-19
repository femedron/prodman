using ProductManager.Repositories.DbModels.Enums;

namespace ProductManager.Repositories.DbModels;

/// <summary>
/// DB-модель складу — зберігає сирі дані так, як вони лежать у сховищі.
/// Не містить обчислюваних полів і колекцій дочірніх сутностей.
/// </summary>
public class WarehouseDbModel
{
    public Guid Id { get; }

    public string Name { get; set; }

    public WarehouseLocation Location { get; set; }

    public WarehouseDbModel(Guid id, string name, WarehouseLocation location)
    {
        Id = id;
        Name = name;
        Location = location;
    }

    public WarehouseDbModel(string name, WarehouseLocation location)
        : this(Guid.NewGuid(), name, location) { }
}
