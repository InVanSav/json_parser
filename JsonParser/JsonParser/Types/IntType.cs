using System.Reflection;
using System.Text.Json;

namespace JsonParser.Types;

/// <summary>
/// Тип целого числа для парсинга JSON
/// </summary>
public class IntType : ParsableType
{
    // <inheritdoc />
    public override bool CanParse(PropertyInfo prop)
        => prop.PropertyType == typeof(int);

    // <inheritdoc />
    public override dynamic? Parse(PropertyInfo prop, JsonElement jsonProp, List<string> errors)
        => jsonProp.TryGetInt32(out var intValue) ? intValue : null;
}