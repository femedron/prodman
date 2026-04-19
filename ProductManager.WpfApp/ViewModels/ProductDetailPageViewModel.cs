using System.Windows.Input;
using ProductManager.ViewModels;
using ProductManager.WpfApp.Infrastructure;

namespace ProductManager.WpfApp.ViewModels;

/// <summary>
/// ViewModel for the product detail page.
/// Receives a <see cref="ProductViewModel"/> and exposes it for display.
/// </summary>
public class ProductDetailPageViewModel : ViewModelBase
{
    private readonly INavigationService _navigation;

    public ProductViewModel Product { get; }

    public ICommand GoBackCommand { get; }

    public ProductDetailPageViewModel(ProductViewModel product, INavigationService navigation)
    {
        Product = product;
        _navigation = navigation;
        GoBackCommand = new RelayCommand(() => _navigation.GoBack());
    }
}
