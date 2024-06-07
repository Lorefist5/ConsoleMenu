using System.Dynamic;

namespace ConsoleMenu.Models;

public class ConsoleItem
{
    
    public ConsoleItem(string name, Action? action = null, List<ConsoleItem>? subItems = null, bool isSpecialCommand = false)
    {
        Name = name;
        if (action != null && subItems != null)
        {
            throw new ArgumentException("Action and SubItems cannot be set at the same time");
        }
        if (action == null && subItems == null)
        {
            throw new ArgumentException("Action and SubItems cannot be null at the same time");
        }
        if (subItems != null)
        {
            HasSubMenu = true;
        }

        Action = action;
        SubItems = subItems;
        IsSpecialCommand = isSpecialCommand;
    }

    public string Name { get; set; } = default!;
    public bool HasSubMenu { get; set; }
    public Action? Action { get; set; }
    public List<ConsoleItem>? SubItems { get; set; } = new();
    public bool IsSpecialCommand { get; }

    public static ConsoleItem CreateWithAction(string name, Action action)
    {
        return new ConsoleItem(name, action, null, false);
    }
    public static ConsoleItem CreateSpecialItem(string name, Action action)
    {
        return new ConsoleItem(name, action, null, true);
    }
    public static ConsoleItem CreateSubMenu(string name, List<ConsoleItem> consoleItems)
    {
        return new ConsoleItem(name, null, consoleItems);
    }
}
