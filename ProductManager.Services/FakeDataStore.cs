using ProductManager.Models;
using ProductManager.Models.Enums;

namespace ProductManager.Services;

/// <summary>
/// Static fake data store that acts as the in-memory database.
/// 
/// Access is intentionally internal so that only <see cref="WarehouseRepository"/>
/// can read or modify data — external code must go through the service layer.
/// 
/// </summary>
internal static class FakeDataStore
{
    // ── Warehouse IDs ─────────────────────────────────────────────────────────
    internal static readonly Guid CentralWarehouseId  = new("aaaaaaaa-0000-0000-0000-000000000001");
    internal static readonly Guid LvivWarehouseId     = new("aaaaaaaa-0000-0000-0000-000000000002");
    internal static readonly Guid ZaporizhzhiaWarehouseId = new("aaaaaaaa-0000-0000-0000-000000000003");

    // ── Warehouses ────────────────────────────────────────────────────────────
    internal static readonly List<WarehouseModel> Warehouses = new()
    {
        new WarehouseModel(CentralWarehouseId,       "Центральний склад",  WarehouseLocation.Kyiv),
        new WarehouseModel(LvivWarehouseId,           "Склад #2 Львів",    WarehouseLocation.Lviv),
        new WarehouseModel(ZaporizhzhiaWarehouseId,   "Регіональний склад", WarehouseLocation.Zaporizhzhia),
    };

    // ── Products ──────────────────────────────────────────────────────────────
    // 10 products → CentralWarehouse, 2 products → LvivWarehouse, 0 → Zaporizhzhia
    internal static readonly List<ProductModel> Products = new()
    {
        // ── Central warehouse (10 items) ──────────────────────────────────────
        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000001"),
            CentralWarehouseId,
            "Samsung Galaxy S24 Ultra",
            45, 42_999m,
            ProductCategory.Electronics,
            "6.8\" AMOLED, Snapdragon 8 Gen 3, 12 ГБ RAM, 256 ГБ"),

        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000002"),
            CentralWarehouseId,
            "Apple MacBook Air M3",
            12, 58_500m,
            ProductCategory.Electronics,
            "13.6\", Apple M3, 8 ГБ RAM, 256 ГБ SSD, macOS Sonoma"),

        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000003"),
            CentralWarehouseId,
            "Sony WH-1000XM5",
            78, 10_200m,
            ProductCategory.Electronics,
            "Бездротові навушники з ANC, 30 год роботи, Bluetooth 5.2"),

        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000004"),
            CentralWarehouseId,
            "Adidas Ultraboost 23",
            130, 6_400m,
            ProductCategory.Clothing,
            "Бігові кросівки, підошва Boost, розміри 38-47"),

        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000005"),
            CentralWarehouseId,
            "Куртка зимова Columbia Omni-Heat",
            55, 8_900m,
            ProductCategory.Clothing,
            "Пухова куртка з технологією теплового відображення, розміри S-XXL"),

        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000006"),
            CentralWarehouseId,
            "Кава Lavazza Qualità Rossa 1 кг",
            320, 680m,
            ProductCategory.Food,
            "Мелена кава, суміш арабіки та робусти, інтенсивність 7/10"),

        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000007"),
            CentralWarehouseId,
            "Олія соняшникова Щедрий Дар 5 л",
            500, 295m,
            ProductCategory.Food,
            "Рафінована дезодорована соняшникова олія"),

        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000008"),
            CentralWarehouseId,
            "Стіл офісний IKEA BEKANT",
            18, 11_400m,
            ProductCategory.Furniture,
            "160×80 см, стільниця з дубового шпону, регульована висота"),

        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000009"),
            CentralWarehouseId,
            "Конструктор LEGO Technic Bugatti Chiron",
            24, 6_200m,
            ProductCategory.Toys,
            "3599 деталей, масштаб 1:8, вік 16+, арт. 42083"),

        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000010"),
            CentralWarehouseId,
            "Гантелі регульовані PowerBlock Elite 2-24 кг",
            40, 14_800m,
            ProductCategory.Sports,
            "Швидке регулювання від 2 до 24 кг, ергономічна ручка"),

        // ── Lviv warehouse (2 items) ───────────────────────────────────────────
        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000011"),
            LvivWarehouseId,
            "Bosch GSB 18V-55 — Дрель-шуруповерт",
            35, 9_750m,
            ProductCategory.Tools,
            "18В, безщітковий мотор, крутний момент 55 Нм, комплект 2 АКБ"),

        new ProductModel(
            new Guid("bbbbbbbb-0000-0000-0000-000000000012"),
            LvivWarehouseId,
            "Крем Nivea Q10 Plus антивіковий 50 мл",
            200, 340m,
            ProductCategory.Cosmetics,
            "Денний крем з коензимом Q10 та про-ретинолом, SPF 30"),
    };
}
