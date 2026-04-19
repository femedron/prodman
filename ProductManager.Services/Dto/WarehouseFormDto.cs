namespace ProductManager.Services.Dto;

/// <summary>
/// DTO для форми створення/редагування складу.
/// Містить лише редаговані поля — Id не редагується, тому він optional.
/// </summary>
public class WarehouseFormDto
{
    /// <summary>Null при створенні нового складу; заповнений при редагуванні.</summary>
    public Guid? Id { get; init; }

    public string Name     { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
}
