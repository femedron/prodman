namespace ProductManager.Services.Dto;

/// <summary>
/// DTO для відображення товару у списку складу.
/// Містить лише поля, потрібні для компактного рядка таблиці.
/// </summary>
public class ProductListDto
{
    public Guid    Id         { get; init; }
    public string  Name       { get; init; } = string.Empty;
    public string  Category   { get; init; } = string.Empty;
    public int     Quantity   { get; init; }
    public decimal UnitPrice  { get; init; }

    /// <summary>UnitPrice * Quantity — обчислюється сервісом.</summary>
    public decimal TotalValue { get; init; }
}
