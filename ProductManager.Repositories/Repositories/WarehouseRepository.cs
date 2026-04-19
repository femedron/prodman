using ProductManager.Repositories.DbModels;
using ProductManager.Repositories.Storage;

namespace ProductManager.Repositories.Repositories;

/// <summary>
/// Конкретна реалізація <see cref="IWarehouseRepository"/>, що працює
/// з <see cref="FakeStorage"/>. У наступних лабораторних може бути замінена
/// на реалізацію з реальною БД без змін у сервісному шарі.
/// </summary>
public class WarehouseRepository : IWarehouseRepository
{
    public IEnumerable<WarehouseDbModel> GetAll()
        => FakeStorage.Warehouses;

    public WarehouseDbModel? GetById(Guid id)
        => FakeStorage.Warehouses.FirstOrDefault(w => w.Id == id);
}
