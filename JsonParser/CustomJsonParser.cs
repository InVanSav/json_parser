using System.Reflection;
using System.Text.Json;
using JsonParser.Attributes;
using JsonParser.Types;

namespace JsonParser;

/// <summary>
/// Кастомный парсер JSON строки в объект
/// </summary>
public static class CustomJsonParser
{
    /// <summary>
    /// Хранит в себе все типы, которые парсер может осилить. Типы являются наследниками <see cref="ParsableType"/>
    /// </summary>
    private static IReadOnlyCollection<ParsableType> _parsableTypes;

    /// <summary>
    /// <inheritdoc cref="CustomJsonParser"/>
    /// </summary>
    static CustomJsonParser()
    {
        var oneTimeCodeInheritors = Assembly
            .GetAssembly(typeof(ParsableType))!
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ParsableType)) && !t.IsAbstract)
            .Select(t => (ParsableType)Activator.CreateInstance(t)!);

        _parsableTypes = oneTimeCodeInheritors.ToArray();
    }

    /// <summary>
    /// Выполнить десериализацию
    /// </summary>
    /// <param name="jsonString">Строка типа JSON</param>
    /// <typeparam name="T">Тип объекта, в который будет десериализована строка</typeparam>
    /// <returns><see cref="JsonParsingResult{T}"/></returns>
    public static JsonParsingResult<T> Deserialize<T>(string jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
            return JsonParsingResult<T>.Failure(new List<string> { "JSON строка не может быть null." });

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
                var parser = _parsableTypes.FirstOrDefault(pt => pt.CanParse(prop));

                var value = parser?.Parse(prop, jsonProp, errors);
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

    private static JsonParsingResult<JsonElement> ParseJson(string jsonString)
    {
        try
        {
            return JsonParsingResult<JsonElement>.Success(JsonDocument.Parse(jsonString).RootElement);
        }
        catch
        {
            return JsonParsingResult<JsonElement>.Failure(new List<string> { "Ошибка парсинга JSON строки." });
        }
    }

    public static MethodInfo? GetDeserializingMethod(Type type)
        => typeof(CustomJsonParser).GetMethod("Deserialize")?.MakeGenericMethod(type);
}