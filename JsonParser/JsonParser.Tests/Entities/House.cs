namespace JsonParser.Tests.Entities;

/// <summary>
/// Дом для тестов
/// </summary>
public record House
{
    [JsonParserPropertyName("address")]
    public string Address { get; init; }

    [JsonParserPropertyName("rooms")]
    public Room[] Rooms { get; init; }

    [JsonParserPropertyName("floors")]
    public int[] Floors { get; init; }

    [JsonParserPropertyName("price")]
    public double Price { get; init; }
}

/// <summary>
/// Комната для тестов
/// </summary>
public record Room
{
    [JsonParserPropertyName("name")]
    public string Name { get; init; }

    [JsonParserPropertyName("area")]
    public double Area { get; init; }

    [JsonParserPropertyName("windows")]
    public int Windows { get; init; }
}