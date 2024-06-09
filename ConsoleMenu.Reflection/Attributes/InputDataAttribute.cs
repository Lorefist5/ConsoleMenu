using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleMenu.Reflection.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class InputDataAttribute : Attribute
{
    public string Prompt { get; set; }
    public string IsRequiredFailureMessage { get; set; } = "The option is required.";
    public string IsNotAcceptableFailureMessage { get; set; } = "The option is not acceptable.";
    public string IsNotEmailFailureMessage { get; set; } = "Invalid email address.";
    public string ValidFormatFailureMessage { get; set; } = "Invalid format.";
    public string IsNotValidRangeMessage { get; set; } = "The value is out of the acceptable range.";
    public string CustomValidationFailureMessage { get; set; } = "The value is not valid.";
    public string IsRegexValidationFailureMessage { get; set; } = "The value is not valid.";
    public bool IsPassword { get; set; } = false;
    public bool IsRequired { get; set; } = false;
    public string[]? AcceptableValues { get; set; }
    public string? DefaultValue { get; set; }
    public bool IsEmail { get; set; } = false;
    public int MinLength { get; set; } = -1;
    public int MaxLength { get; set; } = -1;
    public string? RegexPattern { get; set; } = null; // Use string instead of Regex
    public bool ClearConsoleAfterError { get; set; } = true;
    public bool IsProperty { get; set; } = true;
}
