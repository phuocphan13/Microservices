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
        else
        {
            return string.Empty;
        }
    }
}