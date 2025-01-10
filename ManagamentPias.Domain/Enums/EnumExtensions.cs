using System.ComponentModel;
using System.Reflection;

namespace ManagamentPias.Domain.Enums;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));
        return attribute == null ? value.ToString() : attribute.Description;
    }
}
