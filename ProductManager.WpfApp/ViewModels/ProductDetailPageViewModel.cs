using System.Windows.Input;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Dto;
using ProductManager.WpfApp.Infrastructure;

namespace ProductManager.WpfApp.ViewModels;

/// <summary>
/// ViewModel сторінки деталей товару.
/// Отримує <see cref="ProductListDto"/> зі списку, після чого завантажує
/// повні деталі через <see cref="IProductService"/>.
/// UI знає лише про DTO — не про DbModels.
/// </summary>
public class ProductDetailPageViewModel : ViewModelBase
{
    private readonly INavigationService _navigation;

    private ProductDetailDto? _product;
    private bool _isLoading;

    public ProductDetailDto? Product
    {
        get => _product;
        private set => SetProperty(ref _product, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public ICommand GoBackCommand { get; }

    public ProductDetailPageViewModel(
        ProductListDto listDto,
        IProductService productService,
        INavigationService navigation)
    {
        _navigation = navigation;
        GoBackCommand = new RelayCommand(() => _navigation.GoBack());
        LoadProduct(listDto.Id, productService);
    }

    private void LoadProduct(Guid id, IProductService productService)
    {
        IsLoading = true;
        Product   = productService.GetDetail(id);
        IsLoading = false;
    }
}
