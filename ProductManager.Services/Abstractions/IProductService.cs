using ProductManager.Services.Dto;

namespace ProductManager.Services.Abstractions;

/// <summary>Async-сервіс товарів.</summary>
public interface IProductService
{
    Task<ProductDetailDto?>          GetDetailAsync(Guid id);
    Task<Guid>                       AddAsync(ProductFormDto form);
    Task                             UpdateAsync(ProductFormDto form);
    Task                             DeleteAsync(Guid id);

    /// <summary>Список рядків enum ProductCategory для комбобоксів.</summary>
    IEnumerable<string> GetCategories();
}
