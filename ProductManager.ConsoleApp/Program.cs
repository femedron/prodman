// by Stetsyk M.
using ProductManager.ConsoleApp;
using ProductManager.Services;
using ProductManager.ViewModels;

/*
 * Консольний застосунок для перегляду складів та їхніх товарів.
 *
 * 1. Завантажити список складів через WarehouseRepository.
 * 2. Показати список складів користувачеві.
 * 3. Дати вибрати конкретний склад, показати детальну інформацію.
 * 4. Завантажити товари для вибраного складу.
 * 5. Показати список товарів; дати переглянути повну картку будь-якого товару.
 * 6. Дати можливість повернутись до списку складів або вийти.
 */

namespace ProductManager.ConsoleApp;
internal class Program
{
    static List<WarehouseViewModel>? warehouses = null;
    static WarehouseRepository? repository;
    static void Main(string[] args)
    {

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        repository = new WarehouseRepository();

        while (true)
        {
            ShowWarehouseList();

            var warehouseCount = warehouses!.Count;
            ConsoleHelper.PrintInfo($"0 — Вийти з програми");
            int choice = ConsoleHelper.ReadInt("Оберіть склад", 0, warehouseCount);

            if (choice == 0)
            {
                Console.WriteLine();
                ConsoleHelper.PrintSuccess("До побачення!");
                break;
            }

            var selectedWarehouse = warehouses[choice - 1];
            RunWarehouseDetailLoop(selectedWarehouse);
        }

    }
    // ── Відображення списку складів ───────────────────────────────────────────────
    private static void ShowWarehouseList()
    {
        if (warehouses is null)
        {
            warehouses = repository!.GetAllWarehouses();
            ConsoleHelper.PrintSuccess($"Завантажено {warehouses.Count} складів.");
        }

        ConsoleHelper.PrintHeader("МЕНЕДЖЕР ТОВАРІВ — список складів");

        for (int i = 0; i < warehouses.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {warehouses[i].ToSummaryString()}");
        }
    }

    // ── Деталі складу ────────────────────────────────────────────────────────────
    private static void RunWarehouseDetailLoop(WarehouseViewModel warehouse)
    {
        if (!warehouse.ProductsLoaded)
        {
            repository!.LoadProductsForWarehouse(warehouse);
            ConsoleHelper.PrintSuccess($"Завантажено {warehouse.Products.Count} товарів для складу «{warehouse.Name}».");
        }

        ConsoleHelper.PrintHeader($"ДЕТАЛІ СКЛАДУ: {warehouse.Name}");
        Console.WriteLine(warehouse.ToDetailString());

        if (warehouse.Products.Count == 0)
        {
            ConsoleHelper.PrintInfo("На цьому складі ще немає товарів.");
            ConsoleHelper.PauseForUser();
            return;
        }

        ConsoleHelper.PrintSubHeader("Товари на складі");
        PrintProductList(warehouse.Products);

        // Підменю: переглянути товар детально або повернутись.
        while (true)
        {
            Console.WriteLine();
            ConsoleHelper.PrintInfo($"0 — Повернутись до списку складів");
            ConsoleHelper.PrintInfo($"1-{warehouse.Products.Count} — Переглянути деталі товару");

            int choice = ConsoleHelper.ReadInt("Ваш вибір", 0, warehouse.Products.Count);

            if (choice == 0)
                break;

            var product = warehouse.Products[choice - 1];
            ConsoleHelper.PrintHeader($"ДЕТАЛІ ТОВАРУ: {product.Name}");
            Console.WriteLine(product.ToDetailString());
            ConsoleHelper.PauseForUser();

            // Після перегляду — знову показати список товарів.
            ConsoleHelper.PrintSubHeader("Товари на складі");
            PrintProductList(warehouse.Products);
        }
    }

    // ── Допоміжний метод: вивести пронумерований список товарів ──────────────────
    private static void PrintProductList(List<ProductViewModel> products)
    {
        Console.WriteLine($"  {"#",-4} {"Назва",-35} {"Категорія",-15} {"Кількість",10} {"Ціна/шт",13} {"Загалом",15}");
        Console.WriteLine($"  {new string('-', 100)}");

        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            Console.WriteLine(
                $"  {i + 1,-4} {p.Name,-35} {p.Category,-15} {p.Quantity,8} шт  {p.UnitPrice,10:N2} грн  {p.TotalValue,12:N2} грн");
        }
    }
}