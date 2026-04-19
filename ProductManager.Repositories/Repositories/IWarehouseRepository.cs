using ProductManager.Repositories.DbModels;

namespace ProductManager.Repositories.Repositories;

/// <summary>
/// Абстракція репозиторію складів.
/// Всі методи асинхронні — звертаються до SQLite без блокування потоку.
/// </summary>
public interface IWarehouseRepository
{
    Task<IEnumerable<WarehouseDbModel>> GetAllAsync();
    Task<WarehouseDbModel?> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(WarehouseDbModel warehouse);
    Task UpdateAsync(WarehouseDbModel warehouse);
    Task DeleteAsync(Guid id);
}
