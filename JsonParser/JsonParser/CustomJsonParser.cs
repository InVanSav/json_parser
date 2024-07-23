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
        var errors = new List<string>();
        var result = (T)Activator.CreateInstance(typeof(T))!;

        var jsonObject = JsonDocument.Parse(jsonString).RootElement;

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
        else if (IsComplexType(prop))
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

    private static bool IsComplexType(PropertyInfo prop)
        => prop.PropertyType is { IsClass: true, IsPrimitive: false } && prop.PropertyType != typeof(string);

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