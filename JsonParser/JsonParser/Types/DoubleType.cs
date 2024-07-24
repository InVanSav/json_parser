using System.Reflection;
using System.Text.Json;

namespace JsonParser.Types;

/// <summary>
/// Тип числа с плавающей точкой для парсинга JSON
/// </summary>
public class DoubleType : ParsableType
{
    // <inheritdoc />
    public override bool CanParse(PropertyInfo prop)
        => prop.PropertyType == typeof(double);

    // <inheritdoc />
    public override dynamic? Parse(PropertyInfo prop, JsonElement jsonProp, List<string> errors)
        => jsonProp.TryGetDouble(out var doubleValue) ? doubleValue : null;
}