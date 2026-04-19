using System.Windows;
using ProductManager.Services.Dto;

namespace ProductManager.WpfApp.Views.Dialogs;

/// <summary>
/// Діалог створення та редагування складу.
/// Code-Behind доречний тут: діалог — це View-рівень взаємодії з UI
/// (ShowDialog, DialogResult), що не потребує окремого ViewModel.
/// </summary>
public partial class WarehouseFormDialog : Window
{
    /// <summary>Заповнений після натискання "Зберегти".</summary>
    public WarehouseFormDto? Result { get; private set; }

    private readonly WarehouseFormDto? _existing;

    public WarehouseFormDialog(WarehouseFormDto? existing, IEnumerable<string> locations)
    {
        InitializeComponent();
        _existing = existing;

        TitleText.Text = existing is null ? "Новий склад" : "Редагування складу";

        LocationBox.ItemsSource = locations;

        if (existing is not null)
        {
            NameBox.Text             = existing.Name;
            LocationBox.SelectedItem = existing.Location;
        }
        else
        {
            LocationBox.SelectedIndex = 0;
        }
    }

    private void SaveClick(object sender, RoutedEventArgs e)
    {
        ErrorText.Visibility = Visibility.Collapsed;

        var name = NameBox.Text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            ErrorText.Text       = "Назва складу не може бути порожньою.";
            ErrorText.Visibility = Visibility.Visible;
            return;
        }

        if (LocationBox.SelectedItem is not string location)
        {
            ErrorText.Text       = "Оберіть локацію.";
            ErrorText.Visibility = Visibility.Visible;
            return;
        }

        Result = new WarehouseFormDto
        {
            Id       = _existing?.Id,
            Name     = name,
            Location = location
        };

        DialogResult = true;
    }

    private void CancelClick(object sender, RoutedEventArgs e) =>
        DialogResult = false;
}
