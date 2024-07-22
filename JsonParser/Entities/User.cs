using JsonParser.Attributes;

namespace JsonParser.Entities;

/// <summary>
/// Пользователь для тестов
/// </summary>
public record User
{
    [JsonParserPropertyName("name")]
    public string Name { get; init; }

    [JsonParserPropertyName("age")]
    public int Age { get; init; }

    [JsonParserPropertyName("phone")]
    public string Phone { get; init; }

    [JsonParserPropertyName("address")]
    public Address Address { get; init; }
}

/// <summary>
/// Адрес пользователя для тестов 
/// </summary>
public record Address
{
    [JsonParserPropertyName("street")]
    public string Street { get; init; }

    [JsonParserPropertyName("city")]
    public string City { get; init; }

    [JsonParserPropertyName("zipCode")]
    public string ZipCode { get; init; }
}
