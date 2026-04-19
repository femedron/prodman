namespace ProductManager.Services.Dto;

/// <summary>
/// DTO для детального перегляду складу.
/// Містить усі поля, видимі користувачу, включно з обчислюваними,
/// а також список товарів (у форматі DTO для списку).
/// </summary>
public class WarehouseDetailDto
{
    public Guid   Id           { get; init; }
    public string Name         { get; init; } = string.Empty;
    public string Location     { get; init; } = string.Empty;

    /// <summary>Загальна вартість всіх товарів на складі (обчислюване поле).</summary>
    public decimal TotalValue  { get; init; }

    /// <summary>Список товарів у форматі для відображення у таблиці.</summary>
    public IReadOnlyList<ProductListDto> Products { get; init; } = Array.Empty<ProductListDto>();
}
