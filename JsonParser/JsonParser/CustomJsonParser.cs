using System.Reflection;
using System.Text.Json;

namespace JsonParser;

/// <summary>
/// Кастомный парсер JSON строки в объект
/// </summary>
public static class CustomJsonParser
{
    /// <summary>
    /// Выполнить десериализацию
    /// </summary>
    /// <param name="jsonString">Строка типа JSON</param>
    /// <typeparam name="T">Тип объекта, в который будет десериализована строка</typeparam>
    /// <returns><see cref="JsonParsingResult{T}"/></returns>
    public static JsonParsingResult<T> Deserialize<T>(string jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
            return JsonParsingResult<T>.Failure(new List<string> {"JSON строка не может быть null."});

        var jsonParsingResult = ParseJson(jsonString);
        if (!jsonParsingResult.IsSuccess)
            return JsonParsingResult<T>.Failure(jsonParsingResult.ErrorMessages);

        var errors = new List<string>();
        var jsonObject = jsonParsingResult.Data;
        var result = (T)Activator.CreateInstance(typeof(T))!;

        foreach (var prop in typeof(T).GetProperties())
        {
            var attr = prop.GetCustomAttribute<JsonParserPropertyNameAttribute>();
            if (attr is null)
            {
                errors.Add($"Аттрибут свойства {prop.Name}: не найдено.");
                continue;
            }

            if (!jsonObject.TryGetProperty(attr.Name, out var jsonProp))
            {
                errors.Add($"Свойство {prop.Name}: не найдено в JSON.");
                continue;
            }

            try
            {
                var value = GetValue(prop, jsonProp, errors);
                if (value is not null)
                    prop.SetValue(result, value);
            }
            catch (Exception ex)
            {
                errors.Add($"Свойство {prop.Name}: {ex.Message}");
            }
        }

        return errors.Count == 0
            ? JsonParsingResult<T>.Success(result)
            : JsonParsingResult<T>.Failure(errors);
    }

    private static dynamic? GetValue(PropertyInfo prop, JsonElement jsonProp, List<string> errors)
    {
        dynamic? value = null;

        if (prop.PropertyType == typeof(string))
        {
            value = prop.Name.Equals("phoneNumberText", StringComparison.OrdinalIgnoreCase)
                ? ParsePhoneNumber(jsonProp, errors)
                : ParseString(jsonProp, errors, prop.Name);
        }
        else if (prop.PropertyType == typeof(int))
        {
            if (jsonProp.TryGetInt32(out var intValue))
                value = intValue;
            else
                errors.Add($"Свойство {prop.Name}: не является целочисленным.");
        }
        else if (prop.PropertyType == typeof(double))
        {
            if (jsonProp.TryGetDouble(out var doubleValue))
                value = doubleValue;
            else
                errors.Add($"Свойство {prop.Name}: не является числом с плавающей запятой.");
        }
        else if (IsEnumerableType(prop))
        {
            value = ParseEnumerable(prop, jsonProp, errors);
        }
        else if (IsComplexType(prop.PropertyType))
        {
            var method = typeof(CustomJsonParser).GetMethod("Deserialize")?.MakeGenericMethod(prop.PropertyType);
            var sanitizeResult = method?.Invoke(null, new object[] { jsonProp.GetRawText() }) as dynamic;

            if (sanitizeResult?.IsSuccess)
                value = sanitizeResult.Data;
            else
                errors.AddRange(sanitizeResult.ErrorMessages);
        }

        return value;
    }

    private static bool IsComplexType(Type type)
        => type is { IsPrimitive: false, IsClass: true } && type != typeof(string);

    private static bool IsEnumerableType(PropertyInfo prop)
        => prop.PropertyType.GetInterface(typeof(IEnumerable<>).Name) != null && prop.PropertyType != typeof(string);

    private static JsonParsingResult<JsonElement> ParseJson(string jsonString)
    {
        try
        {
            return JsonParsingResult<JsonElement>.Success(JsonDocument.Parse(jsonString).RootElement);
        }
        catch
        {
            return JsonParsingResult<JsonElement>.Failure(new List<string> {"Ошибка парсинга JSON строки."});
        }
    }

    private static dynamic? ParseEnumerable(PropertyInfo prop, JsonElement jsonProp, List<string> errors)
    {
        var elementType = prop.PropertyType.IsArray
            ? prop.PropertyType.GetElementType()
            : prop.PropertyType.GetGenericArguments().FirstOrDefault();

        if (elementType == null)
        {
            errors.Add($"Свойство {prop.Name}: не удалось определить тип элементов.");
            return null;
        }

        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (dynamic)Activator.CreateInstance(listType)!;

        foreach (var element in jsonProp.EnumerateArray())
        {
            var method = typeof(CustomJsonParser).GetMethod("GetValueForEnumerableElement")?.MakeGenericMethod(elementType);
            var elementResult = method?.Invoke(null, new object[] { element, errors }) as dynamic;

            if (elementResult is not null)
                list.Add(elementResult);
        }

        return list;
    }

    public static dynamic? GetValueForEnumerableElement<T>(JsonElement element, List<string> errors)
    {
        if (typeof(T) == typeof(string))
        {
            return ParseString(element, errors, "Element");
        }

        if (typeof(T) == typeof(int))
        {
            if (element.TryGetInt32(out var intValue))
                return intValue;

            errors.Add("Элемент списка: не является целочисленным.");
        }
        else if (typeof(T) == typeof(double))
        {
            if (element.TryGetDouble(out var doubleValue))
                return doubleValue;

            errors.Add("Элемент списка: не является числом с плавающей запятой.");
        }
        else if (IsComplexType(typeof(T)))
        {
            var method = typeof(CustomJsonParser).GetMethod("Deserialize")?.MakeGenericMethod(typeof(T));
            var sanitizeResult = method?.Invoke(null, new object[] { element.GetRawText() }) as dynamic;

            if (sanitizeResult?.IsSuccess)
                return sanitizeResult.Data;

            errors.AddRange(sanitizeResult.ErrorMessages);
        }

        return default(T);
    }

    private static string? ParsePhoneNumber(JsonElement jsonProp, List<string> errors)
    {
        var str = jsonProp.GetString() ?? string.Empty;

        if (PhoneNumber.TryParse(str, out var phoneNumber))
            return phoneNumber;

        errors.Add("Свойство PhoneNumberText: не является валидным номером телефона.");
        return null;
    }

    private static string ParseString(JsonElement jsonProp, List<string> errors, string propName)
    {
        var str = jsonProp.GetString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(str))
            errors.Add($"Свойство {propName}: строка не может быть пустой или содержать только пробелы.");

        return str.Trim();
    }
}