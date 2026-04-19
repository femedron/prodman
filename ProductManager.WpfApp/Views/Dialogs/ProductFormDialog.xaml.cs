using System.Windows;
using ProductManager.Services.Dto;

namespace ProductManager.WpfApp.Views.Dialogs;

/// <summary>
/// Діалог створення та редагування товару.
/// </summary>
public partial class ProductFormDialog : Window
{
    public ProductFormDto? Result { get; private set; }

    private readonly ProductFormDto? _existing;
    private readonly Guid            _warehouseId;

    public ProductFormDialog(
        ProductFormDto? existing,
        Guid warehouseId,
        IEnumerable<string> categories)
    {
        InitializeComponent();
        _existing    = existing;
        _warehouseId = warehouseId;

        TitleText.Text         = existing is null ? "Новий товар" : "Редагування товару";
        CategoryBox.ItemsSource = categories;

        if (existing is not null)
        {
            NameBox.Text              = existing.Name;
            CategoryBox.SelectedItem  = existing.Category;
            QuantityBox.Text          = existing.Quantity.ToString();
            UnitPriceBox.Text         = existing.UnitPrice.ToString("F2");
            DescriptionBox.Text       = existing.Description;
        }
        else
        {
            CategoryBox.SelectedIndex = 0;
            QuantityBox.Text          = "0";
            UnitPriceBox.Text         = "0.00";
        }
    }

    private void SaveClick(object sender, RoutedEventArgs e)
    {
        ErrorText.Visibility = Visibility.Collapsed;

        var name = NameBox.Text.Trim();
        if (string.IsNullOrEmpty(name))
        { ShowError("Назва товару не може бути порожньою."); return; }

        if (CategoryBox.SelectedItem is not string category)
        { ShowError("Оберіть категорію."); return; }

        if (!int.TryParse(QuantityBox.Text.Trim(), out var qty) || qty < 0)
        { ShowError("Кількість повинна бути цілим невід'ємним числом."); return; }

        if (!decimal.TryParse(
                UnitPriceBox.Text.Trim().Replace(',', '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var price) || price < 0)
        { ShowError("Вкажіть коректну ціну."); return; }

        Result = new ProductFormDto
        {
            Id          = _existing?.Id,
            WarehouseId = _warehouseId,
            Name        = name,
            Category    = category,
            Quantity    = qty,
            UnitPrice   = price,
            Description = DescriptionBox.Text.Trim()
        };

        DialogResult = true;
    }

    private void CancelClick(object sender, RoutedEventArgs e) =>
        DialogResult = false;

    private void ShowError(string msg)
    {
        ErrorText.Text       = msg;
        ErrorText.Visibility = Visibility.Visible;
    }
}
