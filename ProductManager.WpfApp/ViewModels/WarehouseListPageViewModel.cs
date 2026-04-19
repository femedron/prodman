using System.Collections.ObjectModel;
using System.Windows.Input;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Dto;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.Views;

namespace ProductManager.WpfApp.ViewModels;

/// <summary>
/// ViewModel сторінки списку складів.
/// Взаємодіє виключно з <see cref="IWarehouseService"/> — не знає про репозиторії
/// або DbModels.
/// </summary>
public class WarehouseListPageViewModel : ViewModelBase
{
    private readonly IWarehouseService _warehouseService;
    private readonly INavigationService _navigation;
    private readonly Func<WarehouseDetailDto, WarehouseDetailPage> _detailPageFactory;

    private ObservableCollection<WarehouseListDto> _warehouses = new();
    private bool _isLoading;

    public ObservableCollection<WarehouseListDto> Warehouses
    {
        get => _warehouses;
        private set => SetProperty(ref _warehouses, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    /// <summary>Command: navigate to detail page for the selected warehouse.</summary>
    public ICommand OpenWarehouseCommand { get; }

    public WarehouseListPageViewModel(
        IWarehouseService warehouseService,
        INavigationService navigation,
        Func<WarehouseDetailDto, WarehouseDetailPage> detailPageFactory)
    {
        _warehouseService  = warehouseService;
        _navigation        = navigation;
        _detailPageFactory = detailPageFactory;

        OpenWarehouseCommand = new RelayCommand(
            execute:    param => { if (param is WarehouseListDto dto) OpenWarehouse(dto); },
            canExecute: _     => !IsLoading);

        LoadWarehouses();
    }

    private void LoadWarehouses()
    {
        IsLoading = true;
        var items = _warehouseService.GetAll();
        Warehouses = new ObservableCollection<WarehouseListDto>(items);
        IsLoading = false;
    }

    private void OpenWarehouse(WarehouseListDto listDto)
    {
        // Завантажуємо повні деталі складу (разом з товарами) через сервіс.
        var detail = _warehouseService.GetDetail(listDto.Id);
        if (detail is null) return;

        _navigation.NavigateTo(_detailPageFactory(detail));
    }
}
