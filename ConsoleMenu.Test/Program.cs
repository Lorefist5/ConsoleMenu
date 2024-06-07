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

var pathSelected = menu.ExternalValue;

Console.WriteLine();