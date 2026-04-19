using System.Collections.ObjectModel;
using System.Windows.Input;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Dto;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.Views;
using ProductManager.WpfApp.Views.Dialogs;

namespace ProductManager.WpfApp.ViewModels;

/// <summary>
/// ViewModel головної сторінки: список складів + фільтр + сортування + CRUD.
/// </summary>
public class WarehouseListPageViewModel : ViewModelBase //, INavigatedTo
{
    private readonly IWarehouseService _service;
    private readonly INavigationService _nav;
    private readonly Func<WarehouseDetailDto, WarehouseDetailPage> _detailPageFactory;

    private ObservableCollection<WarehouseListDto> _warehouses = new();
    private string _searchText = string.Empty;
    private string _sortField  = "Name";
    private bool   _isBusy;



    public ObservableCollection<WarehouseListDto> Warehouses
    {
        get => _warehouses;
        private set => SetProperty(ref _warehouses, value);
    }

    //public async Task OnNavigatedTo()
    //{
    //    await LoadAsync();
    //}

    public string SearchText
    {
        get => _searchText;
        set { SetProperty(ref _searchText, value); _ = ApplyFilterAsync(); }
    }

    public string SortField
    {
        get => _sortField;
        set { SetProperty(ref _sortField, value); _ = ApplyFilterAsync(); }
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set => SetProperty(ref _isBusy, value);
    }

    public IEnumerable<string> SortOptions { get; } = new[] { "Name", "Location", "ProductCount" };

    public ICommand LoadCommand      { get; }
    public ICommand OpenCommand      { get; }
    public ICommand AddCommand       { get; }
    public ICommand EditCommand      { get; }
    public ICommand DeleteCommand    { get; }

    // Raw list before filter — refreshed on every load
    private List<WarehouseListDto> _allWarehouses = new();

    public WarehouseListPageViewModel(
        IWarehouseService service,
        INavigationService nav,
        Func<WarehouseDetailDto, WarehouseDetailPage> detailPageFactory)
    {
        _service           = service;
        _nav               = nav;
        _detailPageFactory = detailPageFactory;

        LoadCommand   = new AsyncRelayCommand(LoadAsync);
        OpenCommand   = new AsyncRelayCommand(async p => await OpenAsync(p as WarehouseListDto), _ => !IsBusy);
        AddCommand    = new AsyncRelayCommand(AddAsync,  () => !IsBusy);
        EditCommand   = new AsyncRelayCommand(async p => await EditAsync(p as WarehouseListDto), _ => !IsBusy);
        DeleteCommand = new AsyncRelayCommand(async p => await DeleteAsync(p as WarehouseListDto), _ => !IsBusy);
    }

    public async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            _allWarehouses = (await _service.GetAllAsync()).ToList();
            await ApplyFilterAsync();
        }
        finally { IsBusy = false; }
    }

    private Task ApplyFilterAsync()
    {
        var q = _allWarehouses.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var s = SearchText.Trim().ToLower();
            q = q.Where(w => w.Name.ToLower().Contains(s)
                           || w.Location.ToLower().Contains(s));
        }

        q = SortField switch
        {
            "Location"     => q.OrderBy(w => w.Location),
            "ProductCount" => q.OrderByDescending(w => w.ProductCount),
            _              => q.OrderBy(w => w.Name)
        };

        Warehouses = new ObservableCollection<WarehouseListDto>(q);
        return Task.CompletedTask;
    }

    private async Task OpenAsync(WarehouseListDto? dto)
    {
        if (dto is null) return;
        IsBusy = true;
        try
        {
            var detail = await _service.GetDetailAsync(dto.Id);
            if (detail is null) return;
            _nav.NavigateTo(_detailPageFactory(detail));
        }
        finally { IsBusy = false; }
    }

    private async Task AddAsync()
    {
        var locations = _service.GetLocations().ToList();
        var dialog = new WarehouseFormDialog(null, locations);
        if (dialog.ShowDialog() != true) return;

        IsBusy = true;
        try
        {
            await _service.AddAsync(dialog.Result!);
            await LoadAsync();
        }
        finally { IsBusy = false; }
    }

    private async Task EditAsync(WarehouseListDto? dto)
    {
        if (dto is null) return;
        var locations = _service.GetLocations().ToList();
        var current   = new WarehouseFormDto { Id = dto.Id, Name = dto.Name, Location = dto.Location };
        var dialog    = new WarehouseFormDialog(current, locations);
        if (dialog.ShowDialog() != true) return;

        IsBusy = true;
        try
        {
            await _service.UpdateAsync(dialog.Result!);
            await LoadAsync();
        }
        finally { IsBusy = false; }
    }

    private async Task DeleteAsync(WarehouseListDto? dto)
    {
        if (dto is null) return;
        var confirm = System.Windows.MessageBox.Show(
            $"Видалити склад «{dto.Name}»?\nВсі товари цього складу також будуть видалені.",
            "Підтвердження видалення",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning);
        if (confirm != System.Windows.MessageBoxResult.Yes) return;

        IsBusy = true;
        try
        {
            await _service.DeleteAsync(dto.Id);
            await LoadAsync();
        }
        finally { IsBusy = false; }
    }
}
