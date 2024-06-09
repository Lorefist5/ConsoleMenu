
using ConsolePasswordMasker.Core;
using System.Text.RegularExpressions;

namespace ConsoleMenu;

public class ConsoleInput
{
    public required string Prompt { get; set; }
    public string IsRequiredFailureMessage { get; set; } = "The option is required.";
    public string IsNotAcceptableFailureMessage { get; set; } = "The option is not acceptable.";
    public string IsNotEmailFailureMessage { get; set; } = "Invalid email address.";
    public string ValidFormatFailureMessage { get; set; } = "Invalid format.";
    public string IsNotValidRangeMessage { get; set; } = "The value is out of the acceptable range.";
    public string CustomValidationFailureMessage { get; set; } = "The value is not valid.";
    public string IsRegexValidationFailureMessage { get; set; } = "The value is not valid.";
    public bool IsPassword { get; set; } = false;
    public bool IsRequired { get; set; } = false;
    public List<string>? AcceptableValues = null;
    public string? DefaultValue = null;
    public bool IsEmail { get; set; } = false;
    public int MinLength { get; set; } = -1;
    public int MaxLength { get; set; } = -1;
    public Func<string, bool>? CustomValidator { get; set; } = null;
    public Func<string, string>? Formatter { get; set; } = null;
    public TimeSpan? Timeout { get; set; } = null;
    public int? MinRange { get; set; } = null;
    public int? MaxRange { get; set; } = null;
    public Regex? RegexValidator { get; set; } = null;
    public bool ClearConsoleAfterError = true;
    public object Show(Type type)
    {
        try
        {
            var input = Convert.ChangeType(Show(), type);
            return input;
        }
        catch
        {
            Console.WriteLine(ValidFormatFailureMessage);
            return Show(type);
        }
    }
    public T Show<T>()
    {
        try
        {
            T input = (T)Convert.ChangeType(Show(), typeof(T));
            return input;
        }
        catch
        {
            Console.WriteLine(ValidFormatFailureMessage);
            return Show<T>();
        }
    }
    public DateTime ShowDate()
    {
        DateTime selectedDate = DateTime.Today;
        bool done = false;

        while (!done)
        {
            Console.Clear();
            Console.WriteLine(Prompt);
            Console.WriteLine($"Use arrow keys to change date. Press Enter to confirm.");
            Console.WriteLine($"Selected Date: {selectedDate:yyyy-MM-dd}");

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedDate = selectedDate.AddDays(1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedDate = selectedDate.AddDays(-1);
                    break;
                case ConsoleKey.LeftArrow:
                    selectedDate = selectedDate.AddMonths(-1);
                    break;
                case ConsoleKey.RightArrow:
                    selectedDate = selectedDate.AddMonths(1);
                    break;
                case ConsoleKey.Enter:
                    done = true;
                    break;
            }
        }

        return selectedDate;
    }
    public static void PressAnyKeyToContinue(string prompt = "Press any key to continue...")
    {
        Console.WriteLine(prompt);
        Console.ReadKey();
    }
    public string Show()
    {
        return GetPrompt();
    }

    private string GetPrompt()
    {
        if (DefaultValue != null)
        {
            Console.Write($"{Prompt} [{DefaultValue}]: ");
        }
        else
        {
            Console.Write(Prompt);
        }

        string? commandInputResult = IsPassword ? ReadPassword() : ReadInput();

        if (!IsValid(commandInputResult))
        {
            return GetPrompt();
        }

        if (Formatter != null)
        {
            commandInputResult = Formatter(commandInputResult);
        }

        return commandInputResult;
    }

    private string ReadInput()
    {
        if (Timeout.HasValue)
        {
            var task = Task.Run(() => Console.ReadLine());
            if (task.Wait(Timeout.Value))
            {
                return task.Result;
            }
            else
            {
                Console.WriteLine("Input timed out.");
                return string.Empty;
            }
        }
        else
        {
            return Console.ReadLine();
        }
    }

    private bool IsValid(string? value)
    {
        string errorMessage = string.Empty;
        bool isValid = true;
        if (IsRequired && string.IsNullOrWhiteSpace(value))
        {
            errorMessage = IsRequiredFailureMessage;
            isValid = false;
        }

        if (IsEmail && !string.IsNullOrWhiteSpace(value) && !value.Contains("@"))
        {
            errorMessage = IsNotEmailFailureMessage;
            isValid = false;

        }

        if (AcceptableValues != null && !string.IsNullOrWhiteSpace(value) && !AcceptableValues.Contains(value))
        {
            errorMessage = IsNotAcceptableFailureMessage;
            isValid = false;
            
        }

        if (MinLength != -1 && !string.IsNullOrWhiteSpace(value) && value.Length < MinLength)
        {
            errorMessage = IsNotValidRangeMessage;
            isValid = false;
        }

        if (MaxLength != -1 && !string.IsNullOrWhiteSpace(value) && value.Length > MaxLength)
        {
            errorMessage = IsNotValidRangeMessage;
            isValid = false;
        }




        if (CustomValidator != null && !CustomValidator(value))
        {
            errorMessage = CustomValidationFailureMessage;
            isValid = false;
        }

        if (RegexValidator != null && !string.IsNullOrWhiteSpace(value) && !RegexValidator.IsMatch(value))
        {
            errorMessage = ValidFormatFailureMessage;
            isValid = false;
        }
        if(!isValid)
        {
            if(ClearConsoleAfterError)
            {
                Console.Clear();
            }
            Console.WriteLine(errorMessage);
        }
        
        
        return isValid;
    }

    private string ReadPassword()
    {
        PasswordMasker passwordMasker = new();
        var password = passwordMasker.Mask(Prompt);
        return password;
    }
}
