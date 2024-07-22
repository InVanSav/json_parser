using System.Text.RegularExpressions;

namespace JsonParser.JsonParser.Domain.Entities;

/// <summary>
/// Номер телефона для тестов
/// </summary>
public class PhoneNumber
{
    /// <summary>
    /// Номер телефона
    /// </summary>
    public string Phone { get; }

    /// <summary>
    /// <inheritdoc cref="PhoneNumber"/>
    /// </summary>
    public PhoneNumber(string phone)
    {
        Phone = phone;
    }

    /// <summary>
    /// Нормализовать номер телефона
    /// </summary>
    public Result<string> Normalize()
    {
        var match = Regex.Match(Phone, @"^8\s?\(?(\d{3})\)?\s?(\d{3})-?(\d{2})-?(\d{2})$");
        if (!match.Success)
            return Result<string>.Failure("Передан некорректный номер телефона");

        return Result<string>.Success($"7{match.Groups[1].Value}{match.Groups[2].Value}{match.Groups[3].Value}{match.Groups[4].Value}");
    }
}