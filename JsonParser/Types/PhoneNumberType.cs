using System.Reflection;
using System.Text.Json;
using JsonParser.Attributes;

namespace JsonParser.Types;

/// <summary>
/// Тип номера телефона для парсинга JSON
/// </summary>
public class PhoneNumberType : ParsableType
{
    // <inheritdoc />
    public override bool CanParse(PropertyInfo prop)
        => prop.GetCustomAttribute<PhoneNumberPropertyAttribute>() is not null;

    // <inheritdoc />
    public override dynamic? Parse(PropertyInfo prop, JsonElement jsonProp, List<string> errors)
    {
        var str = jsonProp.GetString() ?? string.Empty;

        if (PhoneNumber.TryParse(str, out var phoneNumber))
            return phoneNumber;

        errors.Add("Свойство PhoneNumberText: не является валидным номером телефона.");

        return null;
    }
}