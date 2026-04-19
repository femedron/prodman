using System.Collections.ObjectModel;
using System.Windows.Input;
using ProductManager.Services.Abstractions;
using ProductManager.ViewModels;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.Views;

namespace ProductManager.WpfApp.ViewModels;

/// <summary>
/// ViewModel for the warehouse detail page.
/// Receives a <see cref="WarehouseViewModel"/> from the list page and lazily
/// loads its products if they haven't been loaded yet.
/// </summary>
public class WarehouseDetailPageViewModel : ViewModelBase
{
    private readonly IWarehouseRepository _repository;
    private readonly INavigationService _navigation;
    private readonly Func<ProductViewModel, ProductDetailPage> _productDetailPageFactory;

    private ObservableCollection<ProductViewModel> _products = new();
    private bool _isLoading;

    public WarehouseViewModel Warehouse { get; }

    public ObservableCollection<ProductViewModel> Products
    {
        get => _products;
        private set => SetProperty(ref _products, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public ICommand GoBackCommand { get; }
    public ICommand OpenProductCommand { get; }

    public WarehouseDetailPageViewModel(
        WarehouseViewModel warehouse,
        IWarehouseRepository repository,
        INavigationService navigation,
        Func<ProductViewModel, ProductDetailPage> productDetailPageFactory)
    {
        Warehouse = warehouse;
        _repository = repository;
        _navigation = navigation;
        _productDetailPageFactory = productDetailPageFactory;

        GoBackCommand = new RelayCommand(() => _navigation.GoBack());

        OpenProductCommand = new RelayCommand(
            execute: param =>
            {
                if (param is ProductViewModel pvm)
                    _navigation.NavigateTo(_productDetailPageFactory(pvm));
            });

        LoadProducts();
    }

    private void LoadProducts()
    {
        if (!Warehouse.ProductsLoaded)
        {
            IsLoading = true;
            _repository.LoadProductsForWarehouse(Warehouse);
            IsLoading = false;
        }
        Products = new ObservableCollection<ProductViewModel>(Warehouse.Products);
    }
}
