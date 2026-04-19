using System.Collections.ObjectModel;
using System.Windows.Input;
using ProductManager.Services.Abstractions;
using ProductManager.ViewModels;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.Views;

namespace ProductManager.WpfApp.ViewModels;

/// <summary>
/// ViewModel for the warehouse list page.
/// Loads all warehouses on construction and exposes a command to navigate
/// to a selected warehouse's detail page.
/// </summary>
public class WarehouseListPageViewModel : ViewModelBase
{
    private readonly IWarehouseRepository _repository;
    private readonly INavigationService _navigation;
    private readonly Func<WarehouseViewModel, WarehouseDetailPage> _detailPageFactory;

    private ObservableCollection<WarehouseViewModel> _warehouses = new();
    private WarehouseViewModel? _selectedWarehouse;
    private bool _isLoading;

    public ObservableCollection<WarehouseViewModel> Warehouses
    {
        get => _warehouses;
        private set => SetProperty(ref _warehouses, value);
    }

    public WarehouseViewModel? SelectedWarehouse
    {
        get => _selectedWarehouse;
        set => SetProperty(ref _selectedWarehouse, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public ICommand OpenWarehouseCommand { get; }

    /// <summary>
    /// The <paramref name="detailPageFactory"/> is injected so the ViewModel
    /// never creates WPF Pages directly — Pages are resolved from the DI container.
    /// </summary>
    public WarehouseListPageViewModel(
        IWarehouseRepository repository,
        INavigationService navigation,
        Func<WarehouseViewModel, WarehouseDetailPage> detailPageFactory)
    {
        _repository = repository;
        _navigation = navigation;
        _detailPageFactory = detailPageFactory;

        OpenWarehouseCommand = new RelayCommand(
            execute: param =>
            {
                if (param is WarehouseViewModel vm)
                    OpenWarehouse(vm);
            },
            canExecute: _ => !IsLoading);

        LoadWarehouses();
    }

    private void LoadWarehouses()
    {
        IsLoading = true;
        var list = _repository.GetAllWarehouses();
        Warehouses = new ObservableCollection<WarehouseViewModel>(list);
        IsLoading = false;
    }

    private void OpenWarehouse(WarehouseViewModel warehouse)
    {
        var page = _detailPageFactory(warehouse);
        _navigation.NavigateTo(page);
    }
}
