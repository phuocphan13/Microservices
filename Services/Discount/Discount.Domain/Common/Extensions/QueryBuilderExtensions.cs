using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Discount.Domain.Extensions;

public static class QueryBuilderExtensions
{
    public static string AddParamBuilder(this string query, string propertyName, bool isFirst = false)
    {
        if (!isFirst)
        {
            query += " and ";
        }

        query += $"{propertyName} = @{propertyName}";

        return query;
    }

    public static string CreateQueryBuilder<TEntity>(bool isValueType)
        where TEntity: new()
    {
        List<string> paramsArray = new();
        var entity = new TEntity();

        foreach (var prop in entity.GetType().GetProperties())
        {
            var propName = prop.Name;

            if (propName.ToLower() == "id")
            {
                continue;
            }

            if (isValueType)
            {
                paramsArray.Add($"@{propName}");
            }
            else
            {
                paramsArray.Add($"{propName}");
            }
        }

        return string.Join(",", paramsArray);
    }

    public static string UpdateQueryBuilder<TEntity>()
        where TEntity : new()
    {
        List<string> paramsArray = new();
        var entity = new TEntity();

        foreach (var prop in entity.GetType().GetProperties())
        {
            var propName = prop.Name;

            if (propName.ToLower() == "id")
            {
                continue;
            }

            paramsArray.Add($"{propName}=@{propName}");
        }

        return string.Join(",", paramsArray);
    }

    public static string TablePropertiesBuilder(this object entity)
    {
        List<string> paramsArray = new();

        foreach (var prop in entity.GetType().GetProperties())
        {
            var propName = prop.Name;

            if (propName.ToLower() == "id")
            {
                paramsArray.Add($"{propName} int GENERATED ALWAYS AS IDENTITY PRIMARY KEY");
            }
            else
            {
                var type = GenerateType(prop);

                paramsArray.Add($"{propName} {type}");
            }
        }

        return string.Join(", ", paramsArray);
    }

    private static string GenerateType(PropertyInfo prop)
    {
        var type = prop.PropertyType;

        if (type == typeof(string))
        {
            return GenerateStringType(prop);
        }

        if (type == typeof(int))
        {
            return GenerateIntegerType(prop);
        }

        if (type == typeof(decimal))
        {
            return GenerateDecimalType(prop);
        }

        if (type.BaseType == typeof(Enum))
        {
            return "INT NOT NULL";
        }

        if (type == typeof(bool))
        {
            return "BOOLEAN";
        }

        if (type == typeof(DateTime))
        {
            return "DATE NOT NULL";
        }

        if (type == typeof(DateTime?))
        {
            return "DATE";
        }

        return "";
    }

    private static string GenerateDecimalType(ICustomAttributeProvider prop)
    {
        string type = "";
        var custom = prop.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();

        if (custom is ColumnAttribute columnAttribute)
        {
            type += columnAttribute.TypeName;
        }

        type += GenerateRequired(prop);

        return type;
    }

    private static string GenerateIntegerType(ICustomAttributeProvider prop)
    {
        string type = "INT";

        type += GenerateRequired(prop);

        return type;
    }

    private static string GenerateStringType(ICustomAttributeProvider prop)
    {
        string type = "";
        var custom = prop.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault();

        if (custom is MaxLengthAttribute maxLengthAttr)
        {
            type += $"VARCHAR ({maxLengthAttr.Length})";
        }
        else
        {
            type += $"VARCHAR";
        }

        type += GenerateRequired(prop);

        return type;
    }

    private static string GenerateRequired(ICustomAttributeProvider prop)
    {
        var custom = prop.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault();

        if (custom is not null)
        {
            return " NOT NULL";
        }

        return " NULL";
    }
}