using ConsoleMenu.Reflection.Attributes;
using ConsoleMenu.Reflection.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleMenu.Reflection.Extensions;

public static class ConsoleInputReflection
{
    public static T ShowComplicatedDataType<T>(string initialPrompt) where T : class, new()
    {
        var properties = typeof(T).GetProperties().Where(property => property.GetCustomAttribute<InputDataAttribute>() != null && property.GetCustomAttribute<InputDataAttribute>().IsProperty);
        if (!properties.Any()) throw new Exception("No properties found.");
        T instance = new T();

        Console.WriteLine(initialPrompt);

        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttributes(typeof(InputDataAttribute), false).FirstOrDefault() as InputDataAttribute;
            if (attribute != null)
            {
                var promptTitle = string.IsNullOrWhiteSpace(attribute.Prompt) ? $"Enter {property.Name}: " : attribute.Prompt;

                // Get custom validator if defined
                Func<string, bool>? customValidator = null;
                var customValidatorMethod = typeof(T).GetMethods()
                    .FirstOrDefault(m => m.GetCustomAttribute<InputMethodAttribute>()?.For == property.Name && m.GetCustomAttribute<InputMethodAttribute>()?.Type == TypeOfMethodProperty.Validator);
                if (customValidatorMethod != null)
                {
                    customValidator = (Func<string, bool>)Delegate.CreateDelegate(typeof(Func<string, bool>), customValidatorMethod);
                }

                // Get formatter if defined
                Func<string, string>? formatter = null;
                var formatterMethod = typeof(T).GetMethods()
                    .FirstOrDefault(m => m.GetCustomAttribute<InputMethodAttribute>()?.For == property.Name && m.GetCustomAttribute<InputMethodAttribute>()?.Type == TypeOfMethodProperty.Formatter);
                if (formatterMethod != null)
                {
                    formatter = (Func<string, string>)Delegate.CreateDelegate(typeof(Func<string, string>), formatterMethod);
                }

                var prompt = new ConsoleInput
                {
                    Prompt = promptTitle,
                    IsRequired = attribute.IsRequired,
                    IsRequiredFailureMessage = attribute.IsRequiredFailureMessage,
                    IsNotAcceptableFailureMessage = attribute.IsNotAcceptableFailureMessage,
                    IsNotEmailFailureMessage = attribute.IsNotEmailFailureMessage,
                    ValidFormatFailureMessage = attribute.ValidFormatFailureMessage,
                    IsNotValidRangeMessage = attribute.IsNotValidRangeMessage,
                    CustomValidationFailureMessage = attribute.CustomValidationFailureMessage,
                    IsRegexValidationFailureMessage = attribute.IsRegexValidationFailureMessage,
                    IsPassword = attribute.IsPassword,
                    IsEmail = attribute.IsEmail,
                    MinLength = attribute.MinLength,
                    MaxLength = attribute.MaxLength,
                    MinRange = attribute.MinLength,
                    MaxRange = attribute.MaxLength,
                    RegexValidator = attribute.RegexPattern != null ? new Regex(attribute.RegexPattern) : null,
                    ClearConsoleAfterError = attribute.ClearConsoleAfterError,
                    CustomValidator = customValidator,
                    Formatter = formatter
                };

                object value;
                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    value = prompt.ShowDate();
                }
                else
                {
                    value = prompt.Show(property.PropertyType);
                }
                property.SetValue(instance, Convert.ChangeType(value, property.PropertyType));
            }
        }
        

        return instance;
    }

}