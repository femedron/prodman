using System.Collections.ObjectModel;
using System.Windows.Input;
using ProductManager.Services.Dto;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.Views;

namespace ProductManager.WpfApp.ViewModels;

/// <summary>
/// ViewModel сторінки деталей складу.
/// Отримує <see cref="WarehouseDetailDto"/> (вже повністю заповнений сервісом)
/// і надає його для відображення у View.
/// Не звертається до жодних репозиторіїв або DbModels.
/// </summary>
public class WarehouseDetailPageViewModel : ViewModelBase
{
    private readonly INavigationService _navigation;
    private readonly Func<ProductListDto, ProductDetailPageViewModel> _productVmFactory;

    public WarehouseDetailDto Warehouse { get; }

    public ObservableCollection<ProductListDto> Products { get; }

    public ICommand GoBackCommand    { get; }
    public ICommand OpenProductCommand { get; }

    public WarehouseDetailPageViewModel(
        WarehouseDetailDto warehouse,
        INavigationService navigation,
        Func<ProductListDto, ProductDetailPageViewModel> productVmFactory)
    {
        Warehouse         = warehouse;
        _navigation       = navigation;
        _productVmFactory = productVmFactory;

        Products = new ObservableCollection<ProductListDto>(warehouse.Products);

        GoBackCommand = new RelayCommand(() => _navigation.GoBack());

        OpenProductCommand = new RelayCommand(
            execute: param =>
            {
                if (param is ProductListDto dto)
                {
                    var vm   = _productVmFactory(dto);
                    var page = new ProductDetailPage { DataContext = vm };
                    _navigation.NavigateTo(page);
                }
            });
    }
}
