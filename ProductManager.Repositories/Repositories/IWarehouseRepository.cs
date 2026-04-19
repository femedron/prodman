using ProductManager.Repositories.DbModels;

namespace ProductManager.Repositories.Repositories;

/// <summary>
/// Абстракція для доступу до даних складів.
/// Сервісний рівень взаємодіє з репозиторієм виключно через цей інтерфейс
/// (Dependency Inversion Principle).
/// </summary>
public interface IWarehouseRepository
{
    /// <summary>Повертає всі склади.</summary>
    IEnumerable<WarehouseDbModel> GetAll();

    /// <summary>Повертає склад за ID або null.</summary>
    WarehouseDbModel? GetById(Guid id);
}
