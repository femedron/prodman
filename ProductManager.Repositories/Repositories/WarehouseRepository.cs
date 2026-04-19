using Microsoft.Data.Sqlite;
using ProductManager.Repositories.DbModels;
using ProductManager.Repositories.Storage;

namespace ProductManager.Repositories.Repositories;

/// <summary>
/// Async SQLite-реалізація <see cref="IWarehouseRepository"/>.
/// Каскадне видалення товарів при видаленні складу забезпечується
/// FOREIGN KEY ... ON DELETE CASCADE у схемі БД.
/// </summary>
public class WarehouseRepository : IWarehouseRepository
{
    private readonly SqliteStorage _storage;
    public WarehouseRepository(SqliteStorage storage) => _storage = storage;

    public async Task<IEnumerable<WarehouseDbModel>> GetAllAsync()
    {
        await using var conn = _storage.OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, Location FROM Warehouses ORDER BY Name";
        var results = new List<WarehouseDbModel>();
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync()) results.Add(SqliteStorage.MapWarehouse(r));
        return results;
    }

    public async Task<WarehouseDbModel?> GetByIdAsync(Guid id)
    {
        await using var conn = _storage.OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, Location FROM Warehouses WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id.ToString());
        await using var r = await cmd.ExecuteReaderAsync();
        return await r.ReadAsync() ? SqliteStorage.MapWarehouse(r) : null;
    }

    public async Task<Guid> AddAsync(WarehouseDbModel w)
    {
        await using var conn = _storage.OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Warehouses (Id, Name, Location) VALUES ($id, $name, $loc)";
        cmd.Parameters.AddWithValue("$id",   w.Id.ToString());
        cmd.Parameters.AddWithValue("$name", w.Name);
        cmd.Parameters.AddWithValue("$loc",  w.Location.ToString());
        await cmd.ExecuteNonQueryAsync();
        return w.Id;
    }

    public async Task UpdateAsync(WarehouseDbModel w)
    {
        await using var conn = _storage.OpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Warehouses SET Name = $name, Location = $loc WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id",   w.Id.ToString());
        cmd.Parameters.AddWithValue("$name", w.Name);
        cmd.Parameters.AddWithValue("$loc",  w.Location.ToString());
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var conn = _storage.OpenConnection();
        // Enable FK cascade so child Products are deleted automatically
        var fk = conn.CreateCommand();
        fk.CommandText = "PRAGMA foreign_keys = ON";
        await fk.ExecuteNonQueryAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Warehouses WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id.ToString());
        await cmd.ExecuteNonQueryAsync();
    }
}
