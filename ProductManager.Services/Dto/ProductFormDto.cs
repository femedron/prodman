namespace ProductManager.Services.Dto;

/// <summary>
/// DTO для форми створення/редагування товару.
/// </summary>
public class ProductFormDto
{
    public Guid?   Id          { get; init; }
    public Guid    WarehouseId { get; init; }
    public string  Name        { get; init; } = string.Empty;
    public int     Quantity    { get; init; }
    public decimal UnitPrice   { get; init; }
    public string  Category    { get; init; } = string.Empty;
    public string  Description { get; init; } = string.Empty;
}
