using System.ComponentModel;

namespace Platform.Extensions;

public static class ClassExtensions
{
    public static string GetDescription(this Type type)
    {
        var descriptions = (DescriptionAttribute[])
            type.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (descriptions.Length == 0)
        {
            return string.Empty;
        }

        return descriptions[0].Description;
    }
}