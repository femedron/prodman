using Microsoft.Extensions.DependencyInjection;
using ProductManager.Repositories.Repositories;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Services;

namespace ProductManager.Services;

/// <summary>
/// Розширення для реєстрації всіх сервісів та репозиторіїв у IoC-контейнері.
/// 
/// Єдине місце, де конкретні типи зіставляються з інтерфейсами.
/// Consumers (UI) не знають про конкретні реалізації — лише про інтерфейси.
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductManagerServices(this IServiceCollection services)
    {
        // ── Рівень репозиторіїв ───────────────────────────────────────────────
        // Singleton: FakeStorage є статичним, тому один екземпляр репозиторію
        // на весь час роботи застосунку є коректним.
        services.AddSingleton<IWarehouseRepository, WarehouseRepository>();
        services.AddSingleton<IProductRepository,   ProductRepository>();

        // ── Рівень сервісів ───────────────────────────────────────────────────
        services.AddSingleton<IWarehouseService, WarehouseService>();
        services.AddSingleton<IProductService,   ProductService>();

        return services;
    }
}
