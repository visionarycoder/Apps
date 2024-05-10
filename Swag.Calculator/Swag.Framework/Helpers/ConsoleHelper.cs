using System.Reflection;
using System.Text;

namespace Swag.Framework.Helpers;

public static class ConsoleHelper
{

    public const int DEFAULT_SEPARATOR_WIDTH = 72;

    public static int SeparatorWidth { get; set; } = DEFAULT_SEPARATOR_WIDTH;
    public static char Separator { get; set; } = '-';

    public static string Title { get; set; } = string.Empty;
    public static string Description { get; set; } = string.Empty;

    #region Spinner
    private static readonly string[] spinner = new[] { "|", "/", "-", "\\" };
    private static int spinnerIndex = 0;

    public static void IncrementSpinner()
    {

        if (spinnerIndex > spinner.Length - 1)
        {
            spinnerIndex = 0;
        }
        Console.Write($"{spinner[spinnerIndex++]}");
        Console.CursorLeft -= 1;

    }

    public static void StartSpinner()
    {

        Console.Write(" ");

    }

    public static void StopSpinner()
    {
        Console.WriteLine(" ");
    }
    #endregion Spinner

    #region Highlighting
    private static ConsoleColor foregroundColor;
    private static ConsoleColor backgroundColor;

    private static ConsoleColor highlightedForegroundColor;
    private static ConsoleColor highlightedBackgroundColor;

    public static void ToggleHighlightOn()
    {
        Console.BackgroundColor = highlightedBackgroundColor;
        Console.ForegroundColor = highlightedForegroundColor;
    }

    public static void ToggleHighlightOff()
    {
        Console.BackgroundColor = backgroundColor;
        Console.ForegroundColor = foregroundColor;
    }
    #endregion Highlighting

    #region Reflection
    public static string Display<T>(this T instance, int indentDepth = 0)
    {

        var indent1 = GetIndent(indentDepth++);
        var indent2 = GetIndent(indentDepth++);
        var propertyInfos = GetPropertyInfos(typeof(T));
        var propertyValues = DisplayImp(instance, indentDepth, propertyInfos);
        var results = $"{indent1}Type='{instance.GetName()}';{Environment.NewLine}{indent2}{propertyValues}";

        return results;

    }

    private static string DisplayImp<T>(T instance, int indentDepth, IEnumerable<PropertyInfo> propertyInfos)
    {

        var builder = new StringBuilder();
        foreach (var propertyInfo in propertyInfos)
        {
            var msg = propertyInfo.PropertyType.IsClass
                ? propertyInfo.GetValue(instance).Display(indentDepth: indentDepth++)
                : $"{propertyInfo.Name}='{propertyInfo.GetValue(instance)}'; ";
            builder.Append($"{propertyInfo.Name}='{propertyInfo.GetValue(instance)}'; ");
        }
        return builder.ToString();

    }

    private static string GetName<T>(this T _) => typeof(T).FullName ?? typeof(T).Name;

    private static IEnumerable<PropertyInfo> GetPropertyInfos(Type type)
    {
        return type.GetProperties();
    }

    private static PropertyInfo[] GetPropertyInfos(this Type type, string[] propertyNames)
    {
        return GetPropertyInfos(type).Where(i => propertyNames.Contains(i.Name)).ToArray();
    }
    #endregion Reflection

    #region Input Prompts
    public static int GetIntegerInput(string errorMessage = "Invalid input.  Please try again.")
    {

        do
        {
            var rawInput = Console.ReadLine() ?? string.Empty;
            var trimmedInput = rawInput.Trim();
            if (int.TryParse(trimmedInput, out var value))
            {
                return value;
            }
            Console.WriteLine(errorMessage);
        } while (true);

    }

    public static string GetStringInput(string errorMessage = "Invalid input.  Please try again.")
    {

        do
        {
            var rawInput = Console.ReadLine();
            var trimmedInput = rawInput?.Trim().ToUpperInvariant();
            if (!string.IsNullOrWhiteSpace(trimmedInput))
            {
                return trimmedInput;
            }
            Console.WriteLine(errorMessage);
        } while (true);

    }
    #endregion Input Prompts

    private static string GetIndent(int indentDepth = 0)
    {
        return "".PadRight(indentDepth);
    }

    public static void ShowHeader()
    {
        ShowHeader(Title, Description);
    }

    public static void ShowHeader(string title)
    {
        ShowHeader(title, Description);
    }

    public static void ShowHeader(string title, string description)
    {
        Console.Title = title;
        Console.WriteLine();
        PrintSeparator();
        Console.WriteLine();
        Console.WriteLine(title);
        if (!string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine(description);
        }
        Console.WriteLine();
        PrintSeparator();
        Console.WriteLine();
    }

    public static void ShowFooter()
    {
        Console.WriteLine();
        PrintSeparator();
        Console.WriteLine("Finished.");
        Console.WriteLine();
        PrintSeparator();
        Console.WriteLine();
    }

    public static void ShowExit()
    {
        Console.WriteLine();
        PrintSeparator();
        Console.WriteLine();
        Console.WriteLine("Press any key to exit.");
        Console.WriteLine();
        PrintSeparator();
        Console.WriteLine();
        Console.ReadKey();
    }

    public static void ShowUpdate(string message)
    {
        Console.WriteLine();
        Console.WriteLine(message);
        Console.WriteLine();
    }


    public static void PrintSeparator()
    {
        PrintSeparator(SeparatorWidth, Separator);
    }

    public static void PrintSeparator(int width)
    {
        PrintSeparator(width, Separator);
    }

    public static void PrintSeparator(int width, char paddingChar)
    {
        Console.WriteLine("".PadLeft(width, paddingChar));
    }

    public static void ShowMenu(IEnumerable<KeyValuePair<int,string>> options)
    {
        ShowUpdate("Select an option:");
        var keyWidth = options.Max(i => i.Key.ToString().Length);
        var valueWidth = options.Max(i => i.Value.Length);

        foreach (var option in options)
        {
            var key = option.Key.ToString("N0").PadLeft(keyWidth);
            var value = option.Value.PadRight(valueWidth);
            Console.WriteLine($"{key}: {value}");
        }
    }

    public static (bool Exit, bool invalidInput) GetYesNoInput()
    {

        var input = GetStringInput();
        return input switch
        {
            "Y" => (true,true),
            "N" => (false, true),
            _ => (false,false)
        };

    }

    public static void ShowAsTable(IEnumerable<object> source)
    {
        var src = source.ToList();
        var propertyInfos = source
            .GetType()
            .GetGenericArguments()[0]
            .GetProperties(BindingFlags.Instance & BindingFlags.Public);

        var columnWidths = propertyInfos
            .Select(i => new KeyValuePair<string, int>(i.Name, i.Name.Length))
            .ToDictionary();

        for (var i = 0; i < columnWidths.Keys.Count() ; i++)
        {
            var kvp = columnWidths.ElementAt(i);
            Console.Write($"{kvp.Key} ".PadRight(kvp.Value));
            if (i < columnWidths.Count() - 1)
            {
                Console.Write(" | ");
            }
        }

        foreach (var entry in src)
        {
            foreach (var propertyInfo in propertyInfos)
            {
                Console.Write($"{propertyInfo.GetValue(entry)}");
                if (propertyInfo != propertyInfos.Last())
                {
                    Console.Write(" | ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();

    }

}