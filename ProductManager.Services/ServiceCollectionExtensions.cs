using Microsoft.Extensions.DependencyInjection;
using ProductManager.Repositories.Repositories;
using ProductManager.Repositories.Storage;
using ProductManager.Services.Abstractions;
using ProductManager.Services.Services;

namespace ProductManager.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductManagerServices(this IServiceCollection services)
    {
        // Storage (singleton — one DB connection manager for the app)
        services.AddSingleton<SqliteStorage>();

        // Repositories
        services.AddSingleton<IWarehouseRepository, WarehouseRepository>();
        services.AddSingleton<IProductRepository,   ProductRepository>();

        // Services
        services.AddSingleton<IWarehouseService, WarehouseService>();
        services.AddSingleton<IProductService,   ProductService>();

        return services;
    }
}
