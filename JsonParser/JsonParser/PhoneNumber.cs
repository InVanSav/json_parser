using System.Text.RegularExpressions;

namespace JsonParser;

/// <summary>
/// Номер телефона для нормализации
/// </summary>
public class PhoneNumber
{
    /// <summary>
    /// Строковое представление номера телефона
    /// </summary>
    [JsonParserPropertyName("phoneNumberText")]
    public string PhoneNumberText { get; set; }

    /// <summary>
    /// Регулярное выражение для проверки формата номера телефона
    /// </summary>
    private static readonly Regex PhoneNumberRegex = new Regex(@"^(\+7|8)[\s-]?(\d{3})[\s-]?(\d{3})[\s-]?(\d{2})[\s-]?(\d{2})$", RegexOptions.Compiled);

    /// <summary>
    /// <inheritdoc cref="PhoneNumber"/>
    /// </summary>
    public PhoneNumber()
    {
        PhoneNumberText = "";
    }

    /// <summary>
    /// <inheritdoc cref="PhoneNumber"/>
    /// </summary>
    public PhoneNumber(string phoneNumberText)
    {
        PhoneNumberText = phoneNumberText;
    }

    public static bool TryParse(string input, out string? phoneNumber)
    {
        phoneNumber = null;

        if (string.IsNullOrEmpty(input) || !PhoneNumberRegex.IsMatch(input))
            return false;

        var cleanedInput = input
            .Replace("+", "")
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");

        phoneNumber = cleanedInput;

        return true;
    }
}