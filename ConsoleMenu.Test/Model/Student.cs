using ConsoleMenu.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleMenu.Test.Model;
using ConsoleMenu.Reflection.Attributes;
using ConsoleMenu.Reflection.Enums;
using System;

public class Student
{
    [InputData(Prompt = "Enter student name: ", IsRequired = true, CustomValidationFailureMessage = "Name needs to start with the letter A")]
    public string Name { get; set; }

    [InputData(Prompt = "Enter student email: ", IsEmail = true, IsRequired = true)]
    public string Email { get; set; }

    [InputData(Prompt = "Enter student age: ", IsRequired = true, CustomValidationFailureMessage = "The student can't be below 18 years")]
    public int Age { get; set; }

    [InputData(Prompt = "Enter student password: ", IsPassword = true, IsRequired = true)]
    public string Password { get; set; }

    [InputData(Prompt = "Enter student GPA: ", IsRequired = true)]
    public double GPA { get; set; }

    [InputData(Prompt = "Enter student address: ", IsRequired = true)]
    public string Address { get; set; }

    [InputData(Prompt = "Enter student phone number: ", IsRequired = true)]
    public string PhoneNumber { get; set; }

    [InputData(Prompt = "Enter student major: ", IsRequired = true)]
    public string Major { get; set; }

    [InputData(Prompt = "Enter student enrollment date (YYYY-MM-DD): ", IsRequired = true)]
    public DateTime EnrollmentDate { get; set; }

    [InputData(Prompt = "Enter student graduation date (YYYY-MM-DD): ")]
    public DateTime GraduationDate { get; set; }

    [InputData(Prompt = "Is student on scholarship (yes/no): ", IsRequired = true, AcceptableValues = ["yes", "no"])]
    public string IsOnScholarship { get; set; }

    // Non-prompted properties
    [InputData(IsProperty = false)]
    public bool IsOver18 => Age >= 18;

    [InputData(IsProperty = false)]
    public string HasGoodGPA => GPA >= 3.0 ? "Yes" : "No";

    [InputData(IsProperty = false)]
    public string HashedPassword => new string('*', Password.Length);
    [InputData(IsProperty = false)]
    public bool IsOnScholarshipBool => IsOnScholarship == "yes";
    public override string ToString()
    {
        return $@"
    Student Information
    -------------------
    Name              : {Name}
    Email             : {Email}
    Age               : {Age}
    Password          : {Password}
    GPA               : {GPA}
    Address           : {Address}
    Phone Number      : {PhoneNumber}
    Major             : {Major}
    Enrollment Date   : {EnrollmentDate.ToShortDateString()}
    Graduation Date   : {(GraduationDate.ToShortDateString())}
    On Scholarship    : {(IsOnScholarshipBool ? "Yes" : "No")}
    ";
    }

    // Custom validator for the Name property
    [InputMethod(TypeOfMethodProperty.Validator, "Name")]
    public static bool NameValidator(string input)
    {
        return input.StartsWith("A");
    }

    // Custom formatter for the Name property
    [InputMethod(TypeOfMethodProperty.Formatter, "Name")]
    public static string NameFormatter(string input)
    {
        return input.ToUpper();
    }

    // Custom validator for the Age property
    [InputMethod(TypeOfMethodProperty.Validator, "Age")]
    public static bool AgeValidator(string input)
    {
        return int.TryParse(input, out int age) && age >= 18;
    }
}
