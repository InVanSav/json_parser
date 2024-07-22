using System.Text.RegularExpressions;

namespace JsonParser.Entities;

/// <summary>
/// Номер телефона для тестов
/// </summary>
public static class PhoneNumber
{
    /// <summary>
    /// Регулярное выражение для проверки формата номера телефона
    /// </summary>
    private static readonly Regex PhoneNumberRegex = new Regex(@"^(\+7|8)[\s-]?(\d{3})[\s-]?(\d{3})[\s-]?(\d{2})[\s-]?(\d{2})$", RegexOptions.Compiled);

    public static bool TryParse(string input, out string? phoneNumber)
    {
        phoneNumber = null;

        // Убираем пробелы, скобки и дефисы для проверки
        var cleanedInput = input
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");

        // Проверяем, что строка не пустая и соответствует регулярному выражению
        if (string.IsNullOrEmpty(cleanedInput) || !PhoneNumberRegex.IsMatch(cleanedInput))
            return false;

        phoneNumber = cleanedInput;

        return true;
    }
}