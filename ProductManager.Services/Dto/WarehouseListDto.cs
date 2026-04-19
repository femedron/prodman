namespace ProductManager.Services.Dto;

/// <summary>
/// DTO для відображення складу у списку.
/// Містить лише поля, необхідні для рядка списку — без зайвих даних.
/// </summary>
public class WarehouseListDto
{
    public Guid   Id           { get; init; }
    public string Name         { get; init; } = string.Empty;
    public string Location     { get; init; } = string.Empty;

    /// <summary>
    /// Кількість товарів на складі — обчислюється сервісом на основі даних репозиторію.
    /// </summary>
    public int    ProductCount { get; init; }
}
