using ConsoleMenu;
using ConsoleMenu.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

int minStudentNameLength = 3;
int maxStudentLength = 50;
var students = new List<string>();

void AddStudent()
{
    Console.Clear();
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter student name: ",
        IsRequired = true,
        MinLength = minStudentNameLength,
        MaxLength = maxStudentLength,
        CustomValidator = value => !students.Contains(value),
        CustomValidationFailureMessage = "Student already exists."
    };
    string studentName = prompt.Show();
    students.Add(studentName);
    Console.WriteLine($"Student {studentName} added.");
}

void EditStudent()
{
    Console.Clear();
    if (students.Count == 0)
    {
        Console.WriteLine("No students to edit.");
        return;
    }
    var existingStudentPrompt = new ConsolePrompt
    {
        Prompt = "Enter student name to edit: ",
        IsRequired = true,
        CustomValidator = students.Contains,
        CustomValidationFailureMessage = "Student not found."
    };
    var newStudentNamePrompt = new ConsolePrompt()
    {
        Prompt = "Enter new student name: ",
        IsRequired = true,
        MinLength = 3,
        MaxLength = maxStudentLength,
        CustomValidator = value => !students.Contains(value),
        CustomValidationFailureMessage = "Student already exists."
    };

    string studentName = existingStudentPrompt.Show();
    string newStudentName = newStudentNamePrompt.Show();

    var index = students.IndexOf(studentName);
    students[index] = newStudentName;

}

void DeleteStudent()
{
    Console.Clear();
    if (students.Count == 0)
    {
        Console.WriteLine("No students to delete.");
        return;
    }
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter student name to delete: ",
        IsRequired = true,
        CustomValidator = students.Contains,
        CustomValidationFailureMessage = "Student not found."
    };
    string studentName = prompt.Show();
    students.Remove(studentName);
    
    Console.WriteLine($"Student {studentName} deleted.");

}

void ListStudents()
{
    Console.Clear();
    Console.WriteLine("Listing students:");
    foreach (var student in students)
    {
        Console.WriteLine(student);
    }

    ConsolePrompt.PressAnyKeyToContinue();
    Console.Clear();
}

void SearchStudent()
{
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter student name to search: ",
        IsRequired = true
    };
    string studentName = prompt.Show();

    if (students.Contains(studentName))
    {
        Console.WriteLine($"Student {studentName} found.");
    }
    else
    {
        Console.WriteLine($"Student {studentName} not found.");
    }
}


void RunTests()
{
    ConsoleMenuApp app = new("ConsolePrompt Tests");
    app.AddItem(ConsoleItem.CreateWithAction("Test Required Field", TestRequiredField));
    app.AddItem(ConsoleItem.CreateWithAction("Test Email Validation", TestEmailValidation));
    app.AddItem(ConsoleItem.CreateWithAction("Test Password Field", TestPasswordField));
    app.AddItem(ConsoleItem.CreateWithAction("Test Acceptable Values", TestAcceptableValues));
    app.AddItem(ConsoleItem.CreateWithAction("Test Min Length", TestMinLength));
    app.AddItem(ConsoleItem.CreateWithAction("Test Max Length", TestMaxLength));
    app.AddItem(ConsoleItem.CreateWithAction("Test Int Field", TestIntField));
    app.AddItem(ConsoleItem.CreateWithAction("Test Custom Validator", TestCustomValidator));
    app.AddItem(ConsoleItem.CreateWithAction("Test Formatter", TestFormatter));
    app.AddItem(ConsoleItem.CreateWithAction("Test Timeout", TestTimeout));
    app.AddItem(ConsoleItem.CreateWithAction("Test Regex Validator", TestRegexValidator));
    app.AddItem(ConsoleItem.CreateSubMenu("Test student menu", new()
    {
        ConsoleItem.CreateSpecialItem("Add student", AddStudent),
        ConsoleItem.CreateSpecialItem("Edit student", EditStudent),
        ConsoleItem.CreateSpecialItem("Delete student", DeleteStudent),
        ConsoleItem.CreateSpecialItem("List students", ListStudents),
        ConsoleItem.CreateSpecialItem("Search student", SearchStudent)
    }));
    app.AddItem(ConsoleMenuApp.DefaultOptions.Exit("Exit"));
    app.AddItem(ConsoleMenuApp.DefaultOptions.ClearConsole("Clear console"));
    app.Open();
}

void TestRequiredField()
{
    Console.WriteLine("TestRequiredField:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter your name: ",
        IsRequired = true
    };
    string result = prompt.Show();
    Console.WriteLine($"Result: {result}");
}

void TestEmailValidation()
{
    Console.WriteLine("\nTestEmailValidation:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter your email: ",
        IsEmail = true,
        IsRequired = true
    };
    string result = prompt.Show();
    Console.WriteLine($"Result: {result}");
}

void TestPasswordField()
{
    Console.WriteLine("\nTestPasswordField:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter your password: ",
        IsPassword = true,
        IsRequired = true
    };
    string result = prompt.Show();
    Console.WriteLine($"Result: {result}");
}

void TestAcceptableValues()
{
    Console.WriteLine("\nTestAcceptableValues:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter a color (red, green, blue): ",
        AcceptableValues = new List<string> { "red", "green", "blue" },
        IsRequired = true
    };
    string result = prompt.Show();
    Console.WriteLine($"Result: {result}");
}

void TestMinLength()
{
    Console.WriteLine("\nTestMinLength:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter a username (min 5 characters): ",
        MinLength = 5,
        IsRequired = true
    };
    string result = prompt.Show();
    Console.WriteLine($"Result: {result}");
}

void TestMaxLength()
{
    Console.WriteLine("\nTestMaxLength:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter a short description (max 10 characters): ",
        MaxLength = 10,
        IsRequired = true
    };
    string result = prompt.Show();
    Console.WriteLine($"Result: {result}");
}

void TestIntField()
{
    Console.WriteLine("\nTestIntField:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter a number: ",
        IsRequired = true
    };
    int result = prompt.Show<int>();
    Console.WriteLine($"Result: {result}");
}

void TestCustomValidator()
{
    Console.WriteLine("\nTestCustomValidator:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter a value that starts with 'A': ",
        CustomValidator = value => value.StartsWith("A"),
        ValidFormatFailureMessage = "Value must start with 'A'."
    };
    string result = prompt.Show();
    Console.WriteLine($"Result: {result}");
}

void TestFormatter()
{
    Console.WriteLine("\nTestFormatter:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter a value (will be uppercased): ",
        Formatter = value => value.ToUpper()
    };
    string result = prompt.Show();
    Console.WriteLine($"Result: {result}");
}

void TestTimeout()
{
    Console.WriteLine("\nTestTimeout:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter a value within 5 seconds: ",
        Timeout = TimeSpan.FromSeconds(5)
    };
    string result = prompt.Show();
    Console.WriteLine($"Result: {result}");
}

void TestRegexValidator()
{
    Console.WriteLine("\nTestRegexValidator:");
    var prompt = new ConsolePrompt
    {
        Prompt = "Enter a value that matches '^[a-zA-Z]+$': ",
        RegexValidator = new Regex("^[a-zA-Z]+$"),
        ValidFormatFailureMessage = "Value must only contain letters."
    };
    string result = prompt.Show();
    Console.WriteLine($"Result: {result}");
}

void TestConsoleMenuApp()
{
    var studentMenu = ConsoleItem.CreateSubMenu("Students menu", new()
    {
        ConsoleItem.CreateSpecialItem("Add student", AddStudent),
        ConsoleItem.CreateSpecialItem("Edit student", EditStudent),
        ConsoleItem.CreateSpecialItem("Delete student", DeleteStudent),
        ConsoleItem.CreateSpecialItem("List students", ListStudents),
        ConsoleItem.CreateSpecialItem("Search student", SearchStudent),
        ConsoleItem.CreateSpecialItem("Run ConsolePrompt Tests", RunTests)
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
}

RunTests();
