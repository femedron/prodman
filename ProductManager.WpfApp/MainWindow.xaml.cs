using System.Windows;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.ViewModels;
using ProductManager.WpfApp.Views;

namespace ProductManager.WpfApp;

/// <summary>
/// Code-behind для MainWindow.
/// Відповідальність: підключити NavigationService до Frame та відкрити стартову сторінку.
/// Жодної бізнес-логіки тут немає — вся вона у ViewModels.
/// </summary>
public partial class MainWindow : Window
{
    private readonly NavigationService _navigationService;
    private readonly WarehouseListPageViewModel _listViewModel;

    public MainWindow(
        NavigationService navigationService,
        WarehouseListPageViewModel listViewModel)
    {
        InitializeComponent();
        _navigationService = navigationService;
        _listViewModel     = listViewModel;
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        _navigationService.SetFrame(MainFrame);
        var listPage = new WarehouseListPage { DataContext = _listViewModel };
        _navigationService.NavigateTo(listPage);
    }
}
