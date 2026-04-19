using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Services;
using ProductManager.Services.Dto;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.ViewModels;
using ProductManager.WpfApp.Views;

namespace ProductManager.WpfApp;

/// <summary>
/// Composition Root застосунку.
/// Будує IoC-контейнер і запускає MainWindow.
/// Єдине місце, де конкретні типи зіставляються — всі інші класи
/// отримують залежності через інтерфейси (DIP).
/// </summary>
public partial class App : Application
{
    private IServiceProvider _services = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _services = BuildServiceProvider();
        _services.GetRequiredService<MainWindow>().Show();
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        // ── Рівень Repositories + Services ───────────────────────────────────
        services.AddProductManagerServices();

        // ── Infrastructure ────────────────────────────────────────────────────
        services.AddSingleton<NavigationService>();
        services.AddSingleton<INavigationService>(sp => sp.GetRequiredService<NavigationService>());

        // ── ViewModels ────────────────────────────────────────────────────────
        services.AddTransient<WarehouseListPageViewModel>();

        // Фабрика: WarehouseDetailPageViewModel потребує конкретний WarehouseDetailDto
        services.AddTransient<Func<WarehouseDetailDto, WarehouseDetailPageViewModel>>(sp => dto =>
            new WarehouseDetailPageViewModel(
                dto,
                sp.GetRequiredService<INavigationService>(),
                sp.GetRequiredService<Func<ProductListDto, ProductDetailPageViewModel>>()));

        // Фабрика: ProductDetailPageViewModel потребує конкретний ProductListDto + завантажує деталі
        services.AddTransient<Func<ProductListDto, ProductDetailPageViewModel>>(sp => listDto =>
            new ProductDetailPageViewModel(
                listDto,
                sp.GetRequiredService<Services.Abstractions.IProductService>(),
                sp.GetRequiredService<INavigationService>()));

        // ── Pages ─────────────────────────────────────────────────────────────
        services.AddTransient<WarehouseListPage>();

        services.AddTransient<Func<WarehouseDetailDto, WarehouseDetailPage>>(sp => dto =>
        {
            var vm = sp.GetRequiredService<Func<WarehouseDetailDto, WarehouseDetailPageViewModel>>()(dto);
            return new WarehouseDetailPage { DataContext = vm };
        });

        services.AddTransient<Func<ProductListDto, ProductDetailPage>>(sp => listDto =>
        {
            var vm = sp.GetRequiredService<Func<ProductListDto, ProductDetailPageViewModel>>()(listDto);
            return new ProductDetailPage { DataContext = vm };
        });

        // ── Main window ───────────────────────────────────────────────────────
        services.AddSingleton<MainWindow>();

        return services.BuildServiceProvider();
    }
}
