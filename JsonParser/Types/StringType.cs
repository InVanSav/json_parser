using System.Reflection;
using System.Text.Json;

namespace JsonParser.Types;

/// <summary>
/// Тип строки для парсинга JSON
/// </summary>
public class StringType : ParsableType
{
    // <inheritdoc />
    public override bool CanParse(PropertyInfo prop)
        => prop.PropertyType == typeof(string);

    // <inheritdoc />
    public override dynamic? Parse(PropertyInfo prop, JsonElement jsonProp, List<string> errors)
    {
        var str = jsonProp.GetString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(str))
            errors.Add($"Свойство {prop.Name}: строка не может быть пустой или содержать только пробелы.");

        return str.Trim();
    }
}