using System.ComponentModel;
using System.Reflection;

namespace Platform.Extensions;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum enumValue)
    {
        FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        if (fieldInfo is not null)
        {
            DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }

        return string.Empty;
    }

    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
    
    public static T GetEnumsByDescription<T>(string description)
        where T: Enum
    {
        var enumValues = GetValues<T>();

        foreach (var value in enumValues)
        {
            var enumDesc = value.GetEnumDescription();
            
            if (enumDesc == description)
            {
                return value;
            }
        }

        throw new ArgumentException("Not found.", nameof(description));
    }
}