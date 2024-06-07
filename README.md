# 🖥️ ConsoleMenu 🖥️

ConsoleMenu is a 🚀 powerful and flexible library for creating console-based menus in .NET. It allows you to easily create 🎛️ interactive menus for your console applications.

## 🌟 Features 🌟

- 📜 Create menus with multiple options.
- 🚀 Navigate through different submenus.
- 🎯 Perform actions based on user's selection.
- 📂 Navigate through directories in the file system.

## 📝 Example 📝

Here's an example of how you can use ConsoleMenu to create a menu for managing a list of students:

```cs
using ConsoleMenu;
using ConsoleMenu.Models;

var students = new List<string>();

void AddStudent()
{
    Console.WriteLine("Enter student name:");
    var studentName = Console.ReadLine();
    if(string.IsNullOrWhiteSpace(studentName))
    {
        Console.WriteLine("Student name cannot be empty");
        return;
    }
    students.Add(studentName);
    Console.WriteLine($"Student {studentName} added.");
}

void EditStudent()
{
    Console.WriteLine("Enter student name to edit:");
    var studentName = Console.ReadLine();
    var index = students.IndexOf(studentName);
    if (index != -1)
    {
        Console.WriteLine("Enter new name:");
        var newName = Console.ReadLine();
        students[index] = newName;
        Console.WriteLine($"Student {studentName} edited to {newName}.");
    }
    else
    {
        Console.WriteLine($"Student {studentName} not found.");
    }
}

void DeleteStudent()
{
    Console.WriteLine("Enter student name to delete:");
    var studentName = Console.ReadLine();
    string? studentResult = students.FirstOrDefault(student => student == studentName);
    if (studentResult == null)
    {
        Console.WriteLine($"Student not found");
        return;
    }
    students.Remove(studentResult);
    Console.WriteLine($"Student {studentName} deleted.");
}

void ListStudents()
{
    Console.WriteLine("Listing students:");
    foreach (var student in students)
    {
        Console.WriteLine(student);
    }
}

void SearchStudent()
{
    Console.WriteLine("Enter student name to search:");
    var studentName = Console.ReadLine();
    if (students.Contains(studentName))
    {
        Console.WriteLine($"Student {studentName} found.");
    }
    else
    {
        Console.WriteLine($"Student {studentName} not found.");
    }
}

var studentMenu = ConsoleItem.CreateSubMenu("Students menu", new()
{
    ConsoleItem.CreateSpecialItem("Add student", AddStudent),
    ConsoleItem.CreateSpecialItem("Edit student", EditStudent),
    ConsoleItem.CreateSpecialItem("Delete student", DeleteStudent),
    ConsoleItem.CreateSpecialItem("List students", ListStudents),
    ConsoleItem.CreateSpecialItem("Search student", SearchStudent),
});

string? desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
var menu = new ConsoleMenuApp("Main Menu")
    .AddItem(studentMenu)
    .AddGetFolderOption("Get students folder", desktopPath)
    .AddItem(ConsoleMenuApp.DefaultOptions.ClearConsole("Clear console"))
    .AddItem(ConsoleMenuApp.DefaultOptions.Exit("Exit"));


menu.Open();
```


In this example, a `ConsoleMenuApp` is created with a submenu for managing students and an option for selecting a directory from the file system. The `AddStudent` method is used to add a new student to the list of students. Other student operations can be implemented in a similar way.

## 🚀 Getting Started 🚀

To get started with ConsoleMenu, you can clone this repository and check out the `ConsoleMenu.Test` project, which contains a comprehensive example of how to use the library.
