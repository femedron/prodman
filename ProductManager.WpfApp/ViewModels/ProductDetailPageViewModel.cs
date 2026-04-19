using System.Windows.Input;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Dto;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.Views.Dialogs;

namespace ProductManager.WpfApp.ViewModels;

/// <summary>
/// ViewModel сторінки деталей товару з можливістю редагування.
/// </summary>
public class ProductDetailPageViewModel : ViewModelBase
{
    private readonly IProductService   _productService;
    private readonly INavigationService _nav;

    private ProductDetailDto? _product;
    private bool _isBusy;

    public ProductDetailDto? Product
    {
        get => _product;
        private set => SetProperty(ref _product, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set => SetProperty(ref _isBusy, value);
    }

    public ICommand GoBackCommand  { get; }
    public ICommand EditCommand    { get; }
    public ICommand DeleteCommand  { get; }

    public ProductDetailPageViewModel(
        ProductDetailDto product,
        IProductService productService,
        INavigationService nav)
    {
        _product        = product;
        _productService = productService;
        _nav            = nav;

        GoBackCommand = new RelayCommand(() => _nav.GoBack());
        EditCommand   = new AsyncRelayCommand(EditAsync,   () => !IsBusy);
        DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => !IsBusy);
    }

    private async Task EditAsync()
    {
        if (Product is null) return;
        var categories = _productService.GetCategories().ToList();
        var form = new ProductFormDto
        {
            Id          = Product.Id,
            WarehouseId = Product.WarehouseId,
            Name        = Product.Name,
            Quantity    = Product.Quantity,
            UnitPrice   = Product.UnitPrice,
            Category    = Product.Category,
            Description = Product.Description
        };
        var dialog = new ProductFormDialog(form, Product.WarehouseId, categories);
        if (dialog.ShowDialog() != true) return;

        IsBusy = true;
        try
        {
            await _productService.UpdateAsync(dialog.Result!);
            // Reload updated detail
            Product = await _productService.GetDetailAsync(Product.Id);
        }
        finally { IsBusy = false; }
    }

    private async Task DeleteAsync()
    {
        if (Product is null) return;
        var confirm = System.Windows.MessageBox.Show(
            $"Видалити товар «{Product.Name}»?",
            "Підтвердження",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning);
        if (confirm != System.Windows.MessageBoxResult.Yes) return;

        IsBusy = true;
        try
        {
            await _productService.DeleteAsync(Product.Id);
            _nav.GoBack();
        }
        finally { IsBusy = false; }
    }
}
