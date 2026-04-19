using ProductManager.Repositories.DbModels;
using ProductManager.Repositories.Storage;

namespace ProductManager.Repositories.Repositories;

/// <summary>Async SQLite-реалізація <see cref="IProductRepository"/>.</summary>
public class ProductRepository : IProductRepository
{
    private readonly SqliteStorage _storage;
    public ProductRepository(SqliteStorage storage) => _storage = storage;

    private const string SelectCols =
        "Id, WarehouseId, Name, Quantity, UnitPrice, Category, Description";

    public async Task<IEnumerable<ProductDbModel>> GetByWarehouseIdAsync(Guid warehouseId)
    {
        await using var conn = _storage.OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText =
            $"SELECT {SelectCols} FROM Products WHERE WarehouseId = $wid ORDER BY Name";
        cmd.Parameters.AddWithValue("$wid", warehouseId.ToString());
        var results = new List<ProductDbModel>();
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync()) results.Add(SqliteStorage.MapProduct(r));
        return results;
    }

    public async Task<ProductDbModel?> GetByIdAsync(Guid id)
    {
        await using var conn = _storage.OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT {SelectCols} FROM Products WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id.ToString());
        await using var r = await cmd.ExecuteReaderAsync();
        return await r.ReadAsync() ? SqliteStorage.MapProduct(r) : null;
    }

    public async Task<Guid> AddAsync(ProductDbModel p)
    {
        await using var conn = _storage.OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"""
            INSERT INTO Products ({SelectCols})
            VALUES ($id, $wid, $name, $qty, $price, $cat, $desc)
            """;
        cmd.Parameters.AddWithValue("$id",    p.Id.ToString());
        cmd.Parameters.AddWithValue("$wid",   p.WarehouseId.ToString());
        cmd.Parameters.AddWithValue("$name",  p.Name);
        cmd.Parameters.AddWithValue("$qty",   p.Quantity);
        cmd.Parameters.AddWithValue("$price", (double)p.UnitPrice);
        cmd.Parameters.AddWithValue("$cat",   p.Category.ToString());
        cmd.Parameters.AddWithValue("$desc",  p.Description);
        await cmd.ExecuteNonQueryAsync();
        return p.Id;
    }

    public async Task UpdateAsync(ProductDbModel p)
    {
        await using var conn = _storage.OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = """
            UPDATE Products
            SET Name=$name, Quantity=$qty, UnitPrice=$price,
                Category=$cat, Description=$desc
            WHERE Id=$id
            """;
        cmd.Parameters.AddWithValue("$id",    p.Id.ToString());
        cmd.Parameters.AddWithValue("$name",  p.Name);
        cmd.Parameters.AddWithValue("$qty",   p.Quantity);
        cmd.Parameters.AddWithValue("$price", (double)p.UnitPrice);
        cmd.Parameters.AddWithValue("$cat",   p.Category.ToString());
        cmd.Parameters.AddWithValue("$desc",  p.Description);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = _storage.OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Products WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id.ToString());
        await cmd.ExecuteNonQueryAsync();
    }
}
