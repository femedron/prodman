using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Services;
using ProductManager.ViewModels;
using ProductManager.WpfApp.Infrastructure;
using ProductManager.WpfApp.ViewModels;
using ProductManager.WpfApp.Views;

namespace ProductManager.WpfApp;

/// <summary>
/// Application entry point.
/// Builds the IoC container and launches MainWindow.
/// All services, view-models and pages are registered here following
/// the Composition Root pattern — only this class knows about concrete types.
/// </summary>
public partial class App : Application
{
    private IServiceProvider _services = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _services = BuildServiceProvider();

        var mainWindow = _services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        // ── Domain services (from ProductManager.Services) ────────────────────
        // Registers IWarehouseRepository → WarehouseRepository (singleton)
        services.AddProductManagerServices();

        // ── Infrastructure ────────────────────────────────────────────────────
        // NavigationService is singleton so the same Frame is used everywhere.
        services.AddSingleton<NavigationService>();
        services.AddSingleton<INavigationService>(sp => sp.GetRequiredService<NavigationService>());

        // ── Pages (Transient — a fresh page instance each navigation) ─────────
        services.AddTransient<WarehouseListPage>();
        services.AddTransient<WarehouseDetailPage>();
        services.AddTransient<ProductDetailPage>();

        // ── Page ViewModels ───────────────────────────────────────────────────
        services.AddTransient<WarehouseListPageViewModel>();

        // WarehouseDetailPageViewModel needs a WarehouseViewModel parameter →
        // registered as factory so callers can pass the selected warehouse.
        services.AddTransient<Func<WarehouseViewModel, WarehouseDetailPage>>(sp => warehouse =>
        {
            var nav = sp.GetRequiredService<INavigationService>();
            var repo = sp.GetRequiredService<Services.Abstractions.IWarehouseRepository>();
            var productPageFactory = sp.GetRequiredService<Func<ProductViewModel, ProductDetailPage>>();
            var vm = new WarehouseDetailPageViewModel(warehouse, repo, nav, productPageFactory);
            return new WarehouseDetailPage { DataContext = vm };
        });

        services.AddTransient<Func<ProductViewModel, ProductDetailPage>>(sp => product =>
        {
            var nav = sp.GetRequiredService<INavigationService>();
            var vm = new ProductDetailPageViewModel(product, nav);
            return new ProductDetailPage { DataContext = vm };
        });

        // ── Main window ───────────────────────────────────────────────────────
        services.AddSingleton<MainWindow>();

        return services.BuildServiceProvider();
    }
}
