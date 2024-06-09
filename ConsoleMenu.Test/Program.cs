using ConsoleMenu;
using ConsoleMenu.Models;
using ConsoleMenu.Reflection;
using ConsoleMenu.Reflection.Extensions;
using ConsoleMenu.Test.Model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

int minStudentNameLength = 3;
int maxStudentLength = 50;
var courses = new List<Course>
        {
            new Course
            {
                CourseName = "Introduction to Computer Science",
                CourseCode = "CS101",
                Credits = 3,
                Instructor = "Dr. John Doe",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 12, 15),
                Description = "An introduction to the fundamental concepts of computer science."
            },
            new Course
            {
                CourseName = "Data Structures and Algorithms",
                CourseCode = "CS201",
                Credits = 4,
                Instructor = "Dr. Jane Smith",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 12, 15),
                Description = "A course on data structures and algorithms, covering lists, trees, graphs, and sorting algorithms."
            },
            new Course
            {
                CourseName = "Database Management Systems",
                CourseCode = "CS301",
                Credits = 3,
                Instructor = "Dr. Alan Turing",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 12, 15),
                Description = "An in-depth study of database management systems, including SQL and NoSQL databases."
            },
            new Course
            {
                CourseName = "Operating Systems",
                CourseCode = "CS401",
                Credits = 4,
                Instructor = "Dr. Grace Hopper",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 12, 15),
                Description = "A course on the design and implementation of operating systems."
            }
        };
var students = new List<Student>
        {
            new Student
            {
                Name = "Alice Johnson",
                Email = "alice.johnson@example.com",
                Age = 20,
                Password = "password123",
                GPA = 3.7,
                Address = "123 Main St",
                PhoneNumber = "555-1234",
                Major = "Computer Science",
                EnrollmentDate = new DateTime(2021, 9, 1),
                GraduationDate = new DateTime(2024, 5, 30),
                IsOnScholarship = "yes"
            },
            new Student
            {
                Name = "Andrew Smith",
                Email = "andrew.smith@example.com",
                Age = 22,
                Password = "mypassword",
                GPA = 3.5,
                Address = "456 Elm St",
                PhoneNumber = "555-5678",
                Major = "Mathematics",
                EnrollmentDate = new DateTime(2020, 9, 1),
                GraduationDate = new DateTime(2023, 5, 30),
                IsOnScholarship = "no"
            },
            new Student
            {
                Name = "Amanda Williams",
                Email = "amanda.williams@example.com",
                Age = 21,
                Password = "amandapass",
                GPA = 3.8,
                Address = "789 Oak St",
                PhoneNumber = "555-9012",
                Major = "Physics",
                EnrollmentDate = new DateTime(2021, 9, 1),
                GraduationDate = new DateTime(2024, 5, 30),
                IsOnScholarship = "yes"
            },
            new Student
            {
                Name = "Arthur Brown",
                Email = "arthur.brown@example.com",
                Age = 23,
                Password = "arthurpassword",
                GPA = 3.6,
                Address = "321 Pine St",
                PhoneNumber = "555-3456",
                Major = "Chemistry",
                EnrollmentDate = new DateTime(2019, 9, 1),
                GraduationDate = new DateTime(2022, 5, 30),
                IsOnScholarship = "no"
            }
        };

void AddStudent()
{
    Console.Clear();
    var student = ConsoleInputReflection.ShowComplicatedDataType<Student>("Creating student");
    if(students.Any(students => students.Name == student.Name))
    {
        Console.WriteLine($"Student {student.Name} already exists.");
        return;
    }
    students.Add(student);

    Console.WriteLine($"Student {student.Name} added.");
}

void EditStudent()
{
    Console.Clear();
    if (students.Count == 0)
    {
        Console.WriteLine("No students to edit.");
        return;
    }
    var existingStudentPrompt = new ConsoleInput
    {
        Prompt = "Enter student name to edit: ",
        IsRequired = true,
        CustomValidator = value => students.Any(students => students.Name == value),
        CustomValidationFailureMessage = "Student not found."
    };
    var newStudentNamePrompt = new ConsoleInput()
    {
        Prompt = "Enter new student name: ",
        IsRequired = true,
        MinLength = 3,
        MaxLength = maxStudentLength,
        CustomValidator = value => !students.Any(students => students.Name == value),
        CustomValidationFailureMessage = "Student already exists."
    };

    string studentName = existingStudentPrompt.Show();
    Student student = ConsoleInputReflection.ShowComplicatedDataType<Student>($"Updating student {studentName}: ");

    var fetchedStudentResults = students.FirstOrDefault(students => students.Name == studentName);
    if(fetchedStudentResults == null)
    {
        Console.WriteLine("Student was not found...");
        return;
    }
    fetchedStudentResults.Name = student.Name;
    fetchedStudentResults.Email = student.Email;
    fetchedStudentResults.Age = student.Age;
    fetchedStudentResults.Password = student.Password;
    fetchedStudentResults.GPA = student.GPA;


}

void DeleteStudent()
{
    Console.Clear();
    if (students.Count == 0)
    {
        Console.WriteLine("No students to delete.");
        return;
    }
    var prompt = new ConsoleInput
    {
        Prompt = "Enter student name to delete: ",
        IsRequired = true,
        CustomValidator = value => students.Any(students => students.Name == value),
        CustomValidationFailureMessage = "Student not found."
    };
    string studentName = prompt.Show();
    var fetchedStudentResults = students.FirstOrDefault(students => students.Name == studentName);
    if(fetchedStudentResults == null)
    {
        Console.WriteLine("Student was not found...");
        return;
    }

    students.Remove(fetchedStudentResults);
    
    Console.WriteLine($"Student {studentName} deleted.");

}

void ListStudents()
{
    Console.Clear();
    Console.WriteLine("Listing students:");
    ConsoleOutputReflection.ShowTable(students);
    ConsoleInput.PressAnyKeyToContinue();
    Console.Clear();
}

void SearchStudent()
{
    var prompt = new ConsoleInput
    {
        Prompt = "Enter student name to search: ",
        IsRequired = true
    };
    string studentName = prompt.Show();

    if (students.Any(students => students.Name == studentName))
    {
        Console.WriteLine($"Student {studentName} found.");
        Console.WriteLine($"Student information: {students.FirstOrDefault(students => students.Name == studentName)}");
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
    var prompt = new ConsoleInput
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
    var prompt = new ConsoleInput
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
    var prompt = new ConsoleInput
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
    var prompt = new ConsoleInput
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
    var prompt = new ConsoleInput
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
    var prompt = new ConsoleInput
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
    var prompt = new ConsoleInput
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
    var prompt = new ConsoleInput
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
    var prompt = new ConsoleInput
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
    var prompt = new ConsoleInput
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
    var prompt = new ConsoleInput
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
