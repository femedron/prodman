namespace ProductManager.ConsoleApp;

/// <summary>
/// Shared helper methods for console input/output formatting.
/// Centralises repeated patterns to keep Program.cs clean.
/// </summary>
internal static class ConsoleHelper
{
    private const string Separator = "══════════════════════════════════════════════════════";

    internal static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine(Separator);
        Console.WriteLine($"  {title}");
        Console.WriteLine(Separator);
    }

    internal static void PrintSubHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"── {title} ──────────────────────────────────");
    }

    internal static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  [!] {message}");
        Console.ResetColor();
    }

    internal static void PrintSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  [+] {message}");
        Console.ResetColor();
    }

    internal static void PrintInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  {message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Prompt to press any key before continuing.
    /// </summary>
    internal static void PauseForUser()
    {
        Console.WriteLine();
        Console.WriteLine("  Натисніть будь-яку клавішу для продовження...");
        Console.ReadKey(intercept: true);
    }

    /// <summary>
    /// Reads a non-empty line from the user with a given prompt.
    /// Re-prompts until the user enters something.
    /// </summary>
    internal static string ReadNonEmptyLine(string prompt)
    {
        string? value;
        do
        {
            Console.Write($"  {prompt}: ");
            value = Console.ReadLine()?.Trim();
        }
        while (string.IsNullOrEmpty(value));

        return value;
    }

    /// <summary>
    /// Reads an integer from the user within [min, max].
    /// Re-prompts on invalid input.
    /// </summary>
    internal static int ReadInt(string prompt, int min, int max)
    {
        int result;
        while (true)
        {
            Console.Write($"  {prompt} [{min}-{max}]: ");
            var raw = Console.ReadLine();
            if (int.TryParse(raw, out result) && result >= min && result <= max)
                return result;

            PrintError($"Введіть ціле число від {min} до {max}.");
        }
    }
}
