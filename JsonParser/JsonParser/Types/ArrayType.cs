using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace JsonParser.Types;

/// <summary>
/// Тип массива для парсинга JSON
/// </summary>
public class ArrayType : ParsableType
{
    // <inheritdoc />
    public override bool CanParse(PropertyInfo prop)
        => prop.PropertyType.IsArray;

    // <inheritdoc />
    public override dynamic? Parse(PropertyInfo prop, JsonElement jsonProp, List<string> errors)
    {
        if (jsonProp.ValueKind != JsonValueKind.Array)
        {
            errors.Add("Ожидался массив в JSON, но был найден другой тип.");
            return null;
        }

        var elementType = prop.PropertyType.GetElementType();
        var listType = typeof(List<>).MakeGenericType(elementType!);
        var resultList = Activator.CreateInstance(listType) as IList;
        var method = CustomJsonParser.GetDeserializingMethod(elementType!);

        foreach (var element in jsonProp.EnumerateArray())
        {
            var itemResult = method?.Invoke(null, new object[] { element.GetRawText() }) as dynamic;
            if (itemResult?.IsSuccess is true)
                resultList?.Add(itemResult.Data);
            else
                errors.AddRange(itemResult?.ErrorMessages ?? new List<string>());
        }

        return resultList is not null ? ConvertListToArray(resultList, listType) : null;
    }

    private static object? ConvertListToArray(IList list, Type listType)
    {
        var toArrayMethod = listType.GetMethod("ToArray");
        return toArrayMethod?.Invoke(list, null);
    }
}