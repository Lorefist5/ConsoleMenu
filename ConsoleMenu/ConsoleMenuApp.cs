
using ConsoleMenu.Models;
using Spectre.Console;

namespace ConsoleMenu;

public partial class ConsoleMenuApp
{
    public string ExternalValue { get; set; } = string.Empty;
    public string Title { get; set; }
    private List<ConsoleMenuItem> _items { get; set; } = new();
    private ConsoleMenuApp? Parent { get; set; }
    public ConsoleMenuApp(string title, ConsoleMenuApp? parent = null)
    {
        Title = title;
        Parent = parent;
    }
    public ConsoleMenuApp AddItem(ConsoleItem consoleItem)
    {
        _items.Add(ConsoleMenuItem.MapConsoleItem(this, consoleItem));

        return this;
    }
    public ConsoleMenuApp AddGetFolderOption(string name, string initialPath)
    {
        AddItem(DefaultOptions.GetFolder(this,name,initialPath));
        return this;
    }
    private ConsoleMenuApp AddItem(ConsoleMenuItem consoleMenuItem)
    {
        _items.Add(consoleMenuItem);
        return this;
    }
    public string Open()
    {
        return this.Show().Name;
    }
    private ConsoleMenuItem Show()
    {
        ConsoleMenuItem result;
        do
        {
            if (Parent != null)
            {
                //Add back button if it doesn't exist
                if (_items.All(item => item.Name != "Back")) _items.Add(ConsoleMenuItem.CreateWithAction(this, "Back", () => Parent.Show()));

            }

            var prompt = new SelectionPrompt<ConsoleMenuItem>()
                .Title(this.Title)
                .PageSize(10)
                .AddChoices(this._items)
                .HighlightStyle(new Style(foreground: Color.Black, background: Color.White))
                .UseConverter(item => item.Name);



            result = AnsiConsole.Prompt(prompt);

            result.OnClick();
        }
        while (result.IsSpecialCommand);

        return result;
    }
    private ConsoleMenuItem GoTo(ConsoleMenuItem consoleMenuItem)
    {
        if (consoleMenuItem.SubItems == null)
        {
            throw new ArgumentException("SubItems cannot be null if HasSubMenu is true");
        }

        var submenu = new ConsoleMenuApp(consoleMenuItem.Name, this);
        foreach (var item in consoleMenuItem.SubItems)
        {
            submenu.AddItem(item);
        }
        return submenu.Show();
    }
    
    public static class DefaultOptions
    {
        public static ConsoleItem GetFolder(ConsoleMenuApp menuApp, string name, string initialPath)
        {
            var subItems = new List<ConsoleItem>
    {
        ConsoleItem.CreateWithAction("Select current folder", () =>
        {
            Console.WriteLine($"Selected directory: {initialPath}");
            menuApp.ExternalValue = initialPath;
        }),
        ConsoleItem.CreateSpecialItem("Go to previous folder", () => GetFolder(menuApp, "Previous Folder", System.IO.Directory.GetParent(initialPath)?.FullName ?? initialPath))
    };

            subItems.AddRange(System.IO.Directory.GetDirectories(initialPath)
                .Select(path =>
                {
                var subDirectoryCount = System.IO.Directory.GetDirectories(path).Length;
                var directoryName = System.IO.Path.GetFileName(path);
                return subDirectoryCount > 0 ? GetFolder(menuApp, $"{directoryName}({subDirectoryCount})", path) : ConsoleItem.CreateWithAction($"{directoryName}({subDirectoryCount})", () => menuApp.ExternalValue = path);
                }));

            return ConsoleItem.CreateSubMenu(name, subItems);
        }

        public static ConsoleItem Exit(string name) => ConsoleItem.CreateSpecialItem(name, () => Environment.Exit(0));
        public static ConsoleItem ClearConsole(string name) => ConsoleItem.CreateSpecialItem(name, Console.Clear);


    }
    private class ConsoleMenuItem
    {
        private ConsoleMenuItem(ConsoleMenuApp menu, string name, Action? action = null, List<ConsoleMenuItem>? subItems = null, bool isSpecialCommand = false)
        {
            _menu = menu;
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

        private readonly ConsoleMenuApp _menu = default!;
        public string Name { get; set; } = default!;
        public bool HasSubMenu { get; set; }

        public List<ConsoleMenuItem>? SubItems { get; set; } = default!;
        public bool IsSpecialCommand { get; }
        public Action? Action { get; set; } = default!;
        public ConsoleMenuItem OnClick()
        {
            if (HasSubMenu)
            {
                return _menu.GoTo(this);
            }
            if (Action != null)
            {
                Action();
            }
            return this;
        }
        public static ConsoleMenuItem CreateWithAction(ConsoleMenuApp menu, string name, Action action, bool isSpecialCommand = false)
        {
            return new ConsoleMenuItem(menu, name, action, null, isSpecialCommand);
        }

        public static ConsoleMenuItem CreateWithItems(ConsoleMenuApp menu, string name, List<ConsoleMenuItem> subItems, bool isSpecialCommand = false)
        {
            return new ConsoleMenuItem(menu, name, null, subItems, isSpecialCommand);
        }
        public static ConsoleMenuItem MapConsoleItem(ConsoleMenuApp menu, ConsoleItem item)
        {
            if (item.HasSubMenu)
            {
                if (item.SubItems == null)
                {
                    throw new ArgumentException("SubItems cannot be null if HasSubMenu is true");
                }

                return CreateWithItems(menu, item.Name, item.SubItems.Select(item => MapConsoleItem(menu, item)).ToList(), item.IsSpecialCommand);
            }
            return CreateWithAction(menu, item.Name, item.Action!, item.IsSpecialCommand);
        }
    }
}
