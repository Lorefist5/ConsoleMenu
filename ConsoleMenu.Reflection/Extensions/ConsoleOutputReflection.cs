using System.Reflection;

namespace ConsoleMenu.Reflection.Extensions;

public static class ConsoleOutputReflection
{
    public static void ShowTable<T>(List<T> values)
    {
        if (values == null || !values.Any())
        {
            Console.WriteLine("No data available.");
            return;
        }

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (!properties.Any())
        {
            Console.WriteLine("No properties found.");
            return;
        }

        // Calculate the maximum width for each column
        var headers = properties.Select(p => p.Name).ToArray();
        var maxWidths = headers.Select((header, index) => Math.Max(header.Length, values.Max(v => FormatValue(properties[index].GetValue(v)).Length))).ToArray();

        // Create a header row
        var headerRow = string.Join(" | ", headers.Select((header, index) => header.PadRight(maxWidths[index])));
        var separatorRow = new string('-', headerRow.Length);

        Console.WriteLine(headerRow);
        Console.WriteLine(separatorRow);

        // Create rows for each object
        foreach (var value in values)
        {
            var row = properties.Select((p, index) => FormatValue(p.GetValue(value)).PadRight(maxWidths[index])).ToArray();
            Console.WriteLine(string.Join(" | ", row));
        }
    }

    private static string FormatValue(object value)
    {
        if (value == null)
            return string.Empty;
        if (value is DateTime dateTime)
            return dateTime.ToShortDateString();
        if (value is bool boolean)
            return boolean ? "Yes" : "No";
        return value.ToString();
    }
}
