using System.Windows.Controls;
using ProductManager.WpfApp.ViewModels;
using System.Windows;

namespace ProductManager.WpfApp.Views;

/// <summary>
/// Code-behind WarehouseListPage.
/// Запускає асинхронне завантаження при появі сторінки.
/// </summary>
public partial class WarehouseListPage : Page
{
    public WarehouseListPage() => InitializeComponent();

    protected override async void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        if (DataContext is ViewModels.WarehouseListPageViewModel vm)
            await vm.LoadAsync();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModels.WarehouseListPageViewModel vm)
            await vm.LoadAsync();
    }
}
