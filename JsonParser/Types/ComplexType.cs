using System.Reflection;
using System.Text.Json;

namespace JsonParser.Types;

/// <summary>
/// Сложный тип для парсинга JSON
/// </summary>
public class ComplexType : ParsableType
{
    // <inheritdoc />
    public override bool CanParse(PropertyInfo prop)
        => prop.PropertyType is { IsPrimitive: false, IsClass: true } && prop.PropertyType != typeof(string);

    // <inheritdoc />
    public override dynamic? Parse(PropertyInfo prop, JsonElement jsonProp, List<string> errors)
    {
        dynamic? value = null;

        var method = CustomJsonParser.GetDeserializingMethod(prop.PropertyType);
        var sanitizeResult = method?.Invoke(null, new object[] { jsonProp.GetRawText() }) as dynamic;

        if (sanitizeResult?.IsSuccess)
            value = sanitizeResult.Data;
        else
            errors.AddRange(sanitizeResult.ErrorMessages);

        return value;
    }
}