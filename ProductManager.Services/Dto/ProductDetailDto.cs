namespace ProductManager.Services.Dto;

/// <summary>
/// DTO для детального перегляду товару.
/// Містить усі поля, видимі користувачу на сторінці деталей.
/// </summary>
public class ProductDetailDto
{
    public Guid    Id          { get; init; }
    public Guid    WarehouseId { get; init; }

    /// <summary>Назва складу — додаткове поле, зручне для відображення без додаткового запиту.</summary>
    public string  WarehouseName { get; init; } = string.Empty;

    public string  Name        { get; init; } = string.Empty;
    public string  Category    { get; init; } = string.Empty;
    public int     Quantity    { get; init; }
    public decimal UnitPrice   { get; init; }
    public decimal TotalValue  { get; init; }
    public string  Description { get; init; } = string.Empty;
}
