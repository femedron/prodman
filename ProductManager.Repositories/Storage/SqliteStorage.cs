using Microsoft.Data.Sqlite;
using ProductManager.Repositories.DbModels;
using ProductManager.Repositories.DbModels.Enums;

namespace ProductManager.Repositories.Storage;

/// <summary>
/// Керує SQLite-базою даних застосунку.
/// Відповідальності:
///   1. Ініціалізація схеми (CREATE TABLE IF NOT EXISTS).
///   2. Заповнення початковими даними при першому запуску.
///   3. Надання підключення іншим класам шару.
///
/// Файл БД зберігається поряд з exe: <AppDir>/product_manager.db
/// Не потребує встановлення сторонніх компонентів — Microsoft.Data.Sqlite
/// містить нативний SQLite у NuGet-пакеті.
/// </summary>
public class SqliteStorage
{
    private readonly string _connectionString;

    public SqliteStorage()
    {
        var dbPath = Path.Combine(
            AppContext.BaseDirectory,
            "product_manager.db");

        _connectionString = $"Data Source={dbPath}";
    }

    /// <summary>Відкриває нове підключення до БД.</summary>
    public SqliteConnection OpenConnection()
    {
        var conn = new SqliteConnection(_connectionString);
        conn.Open();
        return conn;
    }

    /// <summary>
    /// Ініціалізує схему і заповнює початкові дані якщо БД порожня.
    /// Має викликатись один раз при старті застосунку.
    /// </summary>
    public async Task InitialiseAsync()
    {
        await CreateSchemaAsync();
        await SeedIfEmptyAsync();
    }

    // ── Schema ────────────────────────────────────────────────────────────────

    private async Task CreateSchemaAsync()
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = """
            PRAGMA journal_mode=WAL;

            CREATE TABLE IF NOT EXISTS Warehouses (
                Id       TEXT PRIMARY KEY,
                Name     TEXT NOT NULL,
                Location TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Products (
                Id          TEXT PRIMARY KEY,
                WarehouseId TEXT NOT NULL,
                Name        TEXT NOT NULL,
                Quantity    INTEGER NOT NULL DEFAULT 0,
                UnitPrice   REAL    NOT NULL DEFAULT 0,
                Category    TEXT    NOT NULL,
                Description TEXT    NOT NULL DEFAULT '',
                FOREIGN KEY (WarehouseId) REFERENCES Warehouses(Id) ON DELETE CASCADE
            );

            PRAGMA foreign_keys = ON;
            """;
        await cmd.ExecuteNonQueryAsync();
    }

    // ── Seed ──────────────────────────────────────────────────────────────────

    private async Task SeedIfEmptyAsync()
    {
        await using var conn = new SqliteConnection(_connectionString);
        await conn.OpenAsync();

        // Check if already seeded
        var checkCmd = conn.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(*) FROM Warehouses";
        var count = Convert.ToInt64(await checkCmd.ExecuteScalarAsync());
        if (count > 0) return;

        // ── Fixed warehouse IDs ────────────────────────────────────────────────
        var centralId      = "aaaaaaaa-0000-0000-0000-000000000001";
        var lvivId         = "aaaaaaaa-0000-0000-0000-000000000002";
        var zaporizhzhiaId = "aaaaaaaa-0000-0000-0000-000000000003";

        await using var tx = await conn.BeginTransactionAsync();

        // Warehouses
        await InsertWarehouseAsync(conn, centralId,      "Центральний склад",  "Kyiv");
        await InsertWarehouseAsync(conn, lvivId,          "Склад №2 Львів",    "Lviv");
        await InsertWarehouseAsync(conn, zaporizhzhiaId,  "Регіональний склад", "Zaporizhzhia");

        // Products — Central (10)
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000001", centralId,
            "Samsung Galaxy S24 Ultra",             45, 42999m, "Electronics",
            "6.8\" AMOLED, Snapdragon 8 Gen 3, 12 ГБ RAM, 256 ГБ");
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000002", centralId,
            "Apple MacBook Air M3",                 12, 58500m, "Electronics",
            "13.6\", Apple M3, 8 ГБ RAM, 256 ГБ SSD, macOS Sonoma");
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000003", centralId,
            "Sony WH-1000XM5",                      78, 10200m, "Electronics",
            "Бездротові навушники з ANC, 30 год роботи, Bluetooth 5.2");
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000004", centralId,
            "Adidas Ultraboost 23",                130,  6400m, "Clothing",
            "Бігові кросівки, підошва Boost, розміри 38-47");
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000005", centralId,
            "Куртка зимова Columbia Omni-Heat",     55,  8900m, "Clothing",
            "Пухова куртка з технологією теплового відображення, S-XXL");
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000006", centralId,
            "Кава Lavazza Qualità Rossa 1 кг",     320,   680m, "Food",
            "Мелена кава, суміш арабіки та робусти, інтенсивність 7/10");
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000007", centralId,
            "Олія соняшникова Щедрий Дар 5 л",    500,   295m, "Food",
            "Рафінована дезодорована соняшникова олія");
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000008", centralId,
            "Стіл офісний IKEA BEKANT",             18, 11400m, "Furniture",
            "160×80 см, стільниця з дубового шпону, регульована висота");
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000009", centralId,
            "Конструктор LEGO Technic Bugatti",     24,  6200m, "Toys",
            "3599 деталей, масштаб 1:8, вік 16+, арт. 42083");
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000010", centralId,
            "Гантелі PowerBlock Elite 2-24 кг",    40, 14800m, "Sports",
            "Швидке регулювання від 2 до 24 кг, ергономічна ручка");

        // Products — Lviv (2)
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000011", lvivId,
            "Bosch GSB 18V-55 Дрель-шуруповерт",  35,  9750m, "Tools",
            "18В, безщітковий мотор, крутний момент 55 Нм, комплект 2 АКБ");
        await InsertProductAsync(conn, "bbbbbbbb-0000-0000-0000-000000000012", lvivId,
            "Крем Nivea Q10 Plus 50 мл",          200,   340m, "Cosmetics",
            "Денний крем з коензимом Q10 та про-ретинолом, SPF 30");

        await tx.CommitAsync();
    }

    private static async Task InsertWarehouseAsync(
        SqliteConnection conn, string id, string name, string location)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText =
            "INSERT INTO Warehouses (Id, Name, Location) VALUES ($id, $name, $loc)";
        cmd.Parameters.AddWithValue("$id",   id);
        cmd.Parameters.AddWithValue("$name", name);
        cmd.Parameters.AddWithValue("$loc",  location);
        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task InsertProductAsync(
        SqliteConnection conn, string id, string warehouseId,
        string name, int qty, decimal price, string category, string desc)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO Products
                (Id, WarehouseId, Name, Quantity, UnitPrice, Category, Description)
            VALUES
                ($id, $wid, $name, $qty, $price, $cat, $desc)
            """;
        cmd.Parameters.AddWithValue("$id",    id);
        cmd.Parameters.AddWithValue("$wid",   warehouseId);
        cmd.Parameters.AddWithValue("$name",  name);
        cmd.Parameters.AddWithValue("$qty",   qty);
        cmd.Parameters.AddWithValue("$price", (double)price);
        cmd.Parameters.AddWithValue("$cat",   category);
        cmd.Parameters.AddWithValue("$desc",  desc);
        await cmd.ExecuteNonQueryAsync();
    }

    // ── Mapping helpers (used by repositories) ────────────────────────────────

    public static WarehouseDbModel MapWarehouse(SqliteDataReader r) => new(
        Guid.Parse(r.GetString(0)),
        r.GetString(1),
        Enum.Parse<WarehouseLocation>(r.GetString(2)));

    public static ProductDbModel MapProduct(SqliteDataReader r) => new(
        Guid.Parse(r.GetString(0)),
        Guid.Parse(r.GetString(1)),
        r.GetString(2),
        r.GetInt32(3),
        (decimal)r.GetDouble(4),
        Enum.Parse<ProductCategory>(r.GetString(5)),
        r.GetString(6));
}
