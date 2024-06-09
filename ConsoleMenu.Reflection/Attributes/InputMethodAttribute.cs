using ConsoleMenu.Reflection.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMenu.Reflection.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class InputMethodAttribute : Attribute
{
    public TypeOfMethodProperty Type { get; set; }
    public string For { get; set; } // The property name this method is for

    public InputMethodAttribute(TypeOfMethodProperty type, string forProperty)
    {
        Type = type;
        For = forProperty;
    }
}