using System.Collections.ObjectModel;
using System.Windows.Input;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Dto;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.Views;
using ProductManager.WpfApp.Views.Dialogs;

namespace ProductManager.WpfApp.ViewModels;

/// <summary>
/// ViewModel сторінки деталей складу: показ полів + список товарів
/// з фільтрацією, сортуванням та CRUD.
/// </summary>
public class WarehouseDetailPageViewModel : ViewModelBase
{
    private readonly IWarehouseService _warehouseService;
    private readonly IProductService   _productService;
    private readonly INavigationService _nav;
    private readonly Func<ProductDetailDto, ProductDetailPage> _productDetailFactory;

    private WarehouseDetailDto _warehouse;
    private ObservableCollection<ProductListDto> _products = new();
    private string _searchText = string.Empty;
    private string _sortField  = "Name";
    private bool   _isBusy;
    private List<ProductListDto> _allProducts = new();

    public WarehouseDetailDto Warehouse
    {
        get => _warehouse;
        private set => SetProperty(ref _warehouse, value);
    }

    public ObservableCollection<ProductListDto> Products
    {
        get => _products;
        private set => SetProperty(ref _products, value);
    }

    public string SearchText
    {
        get => _searchText;
        set { SetProperty(ref _searchText, value); ApplyFilter(); }
    }

    public string SortField
    {
        get => _sortField;
        set { SetProperty(ref _sortField, value); ApplyFilter(); }
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set => SetProperty(ref _isBusy, value);
    }

    public IEnumerable<string> SortOptions { get; } =
        new[] { "Name", "Category", "Quantity", "UnitPrice", "TotalValue" };

    public ICommand GoBackCommand        { get; }
    public ICommand OpenProductCommand   { get; }
    public ICommand AddProductCommand    { get; }
    public ICommand EditProductCommand   { get; }
    public ICommand DeleteProductCommand { get; }
    public ICommand EditWarehouseCommand { get; }

    public WarehouseDetailPageViewModel(
        WarehouseDetailDto warehouse,
        IWarehouseService warehouseService,
        IProductService productService,
        INavigationService nav,
        Func<ProductDetailDto, ProductDetailPage> productDetailFactory)
    {
        _warehouse            = warehouse;
        _warehouseService     = warehouseService;
        _productService       = productService;
        _nav                  = nav;
        _productDetailFactory = productDetailFactory;

        _allProducts = warehouse.Products.ToList();
        ApplyFilter();

        GoBackCommand        = new RelayCommand(() => _nav.GoBack());
        OpenProductCommand   = new AsyncRelayCommand(async p => await OpenProductAsync(p as ProductListDto));
        AddProductCommand    = new AsyncRelayCommand(AddProductAsync,  () => !IsBusy);
        EditProductCommand   = new AsyncRelayCommand(async p => await EditProductAsync(p as ProductListDto), _ => !IsBusy);
        DeleteProductCommand = new AsyncRelayCommand(async p => await DeleteProductAsync(p as ProductListDto), _ => !IsBusy);
        EditWarehouseCommand = new AsyncRelayCommand(EditWarehouseAsync, () => !IsBusy);
    }

    private void ApplyFilter()
    {
        var q = _allProducts.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var s = SearchText.Trim().ToLower();
            q = q.Where(p => p.Name.ToLower().Contains(s)
                           || p.Category.ToLower().Contains(s));
        }

        q = SortField switch
        {
            "Category"   => q.OrderBy(p => p.Category),
            "Quantity"   => q.OrderByDescending(p => p.Quantity),
            "UnitPrice"  => q.OrderByDescending(p => p.UnitPrice),
            "TotalValue" => q.OrderByDescending(p => p.TotalValue),
            _            => q.OrderBy(p => p.Name)
        };

        Products = new ObservableCollection<ProductListDto>(q);
    }

    private async Task RefreshAsync()
    {
        IsBusy = true;
        try
        {
            var updated = await _warehouseService.GetDetailAsync(Warehouse.Id);
            if (updated is null) return;
            Warehouse    = updated;
            _allProducts = updated.Products.ToList();
            ApplyFilter();
        }
        finally { IsBusy = false; }
    }

    private async Task OpenProductAsync(ProductListDto? dto)
    {
        if (dto is null) return;
        IsBusy = true;
        try
        {
            var detail = await _productService.GetDetailAsync(dto.Id);
            if (detail is null) return;
            _nav.NavigateTo(_productDetailFactory(detail));
        }
        finally { IsBusy = false; }
    }

    private async Task AddProductAsync()
    {
        var categories = _productService.GetCategories().ToList();
        var dialog = new ProductFormDialog(null, Warehouse.Id, categories);
        if (dialog.ShowDialog() != true) return;

        IsBusy = true;
        try   { await _productService.AddAsync(dialog.Result!); await RefreshAsync(); }
        finally { IsBusy = false; }
    }

    private async Task EditProductAsync(ProductListDto? dto)
    {
        if (dto is null) return;
        var detail = await _productService.GetDetailAsync(dto.Id);
        if (detail is null) return;

        var categories = _productService.GetCategories().ToList();
        var form = new ProductFormDto
        {
            Id          = detail.Id,
            WarehouseId = detail.WarehouseId,
            Name        = detail.Name,
            Quantity    = detail.Quantity,
            UnitPrice   = detail.UnitPrice,
            Category    = detail.Category,
            Description = detail.Description
        };
        var dialog = new ProductFormDialog(form, Warehouse.Id, categories);
        if (dialog.ShowDialog() != true) return;

        IsBusy = true;
        try   { await _productService.UpdateAsync(dialog.Result!); await RefreshAsync(); }
        finally { IsBusy = false; }
    }

    private async Task DeleteProductAsync(ProductListDto? dto)
    {
        if (dto is null) return;
        var confirm = System.Windows.MessageBox.Show(
            $"Видалити товар «{dto.Name}»?",
            "Підтвердження видалення",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning);
        if (confirm != System.Windows.MessageBoxResult.Yes) return;

        IsBusy = true;
        try   { await _productService.DeleteAsync(dto.Id); await RefreshAsync(); }
        finally { IsBusy = false; }
    }

    private async Task EditWarehouseAsync()
    {
        var locations = _warehouseService.GetLocations().ToList();
        var form = new WarehouseFormDto
        {
            Id       = Warehouse.Id,
            Name     = Warehouse.Name,
            Location = Warehouse.Location
        };
        var dialog = new WarehouseFormDialog(form, locations);
        if (dialog.ShowDialog() != true) return;

        IsBusy = true;
        try   { await _warehouseService.UpdateAsync(dialog.Result!); await RefreshAsync(); }
        finally { IsBusy = false; }
    }
}
