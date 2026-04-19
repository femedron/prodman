using System.Windows;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.ViewModels;
using ProductManager.WpfApp.Views;

namespace ProductManager.WpfApp;

/// <summary>
/// Code-behind for MainWindow.
/// Responsibilities:
///   1. Give the NavigationService a reference to MainFrame after XAML loads.
///   2. Navigate to the initial WarehouseListPage.
/// All other logic lives in ViewModels.
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

        // Wire up the Frame now that the XAML tree is fully built.
        _navigationService.SetFrame(MainFrame);

        // Navigate to the first page.
        var listPage = new WarehouseListPage { DataContext = _listViewModel };
        _navigationService.NavigateTo(listPage);
    }
}
