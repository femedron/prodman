using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Repositories.Storage;
using ProductManager.Services;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Dto;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.ViewModels;
using ProductManager.WpfApp.Views;
using ProductManager.WpfApp.Views.Dialogs;

namespace ProductManager.WpfApp;

/// <summary>
/// Composition Root застосунку.
/// Ініціалізує SQLite-сховище (schema + seed) і запускає MainWindow.
/// </summary>
public partial class App : Application
{
    private IServiceProvider _services = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _services = BuildServiceProvider();

        // Ініціалізація БД до показу UI (schema + seed при першому запуску)
        var storage = _services.GetRequiredService<SqliteStorage>();
        await storage.InitialiseAsync();

        _services.GetRequiredService<MainWindow>().Show();
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        // Repositories + Services (includes SqliteStorage)
        services.AddProductManagerServices();

        // Navigation
        services.AddSingleton<NavigationService>();
        services.AddSingleton<INavigationService>(sp => sp.GetRequiredService<NavigationService>());

        // ── Page factories ────────────────────────────────────────────────────

        // WarehouseDetailPage factory
        services.AddTransient<Func<WarehouseDetailDto, WarehouseDetailPage>>(sp => dto =>
        {
            var vm = new WarehouseDetailPageViewModel(
                dto,
                sp.GetRequiredService<IWarehouseService>(),
                sp.GetRequiredService<IProductService>(),
                sp.GetRequiredService<INavigationService>(),
                sp.GetRequiredService<Func<ProductDetailDto, ProductDetailPage>>());
            return new WarehouseDetailPage { DataContext = vm };
        });

        // ProductDetailPage factory
        services.AddTransient<Func<ProductDetailDto, ProductDetailPage>>(sp => dto =>
        {
            var vm = new ProductDetailPageViewModel(
                dto,
                sp.GetRequiredService<IProductService>(),
                sp.GetRequiredService<INavigationService>());
            return new ProductDetailPage { DataContext = vm };
        });

        // ── ViewModels ────────────────────────────────────────────────────────
        services.AddTransient<WarehouseListPageViewModel>();

        // ── Pages ─────────────────────────────────────────────────────────────
        services.AddTransient<WarehouseListPage>();

        // ── Main window ───────────────────────────────────────────────────────
        services.AddSingleton<MainWindow>();

        return services.BuildServiceProvider();
    }
}
