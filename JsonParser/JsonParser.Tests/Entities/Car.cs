namespace JsonParser.Tests.Entities;

/// <summary>
/// Автомобиль с колесами для тестов
/// </summary>
public record Car
{
    [JsonParserPropertyName("model")] 
    public string Model { get; init; }

    [JsonParserPropertyName("horsePower")] 
    public int HorsePower { get; init; }

    [JsonParserPropertyName("wheelCount")] 
    public int WheelCount { get; init; }

    [JsonParserPropertyName("model")] 
    public IReadOnlyCollection<Wheel> Wheels { get; init; }
}

/// <summary>
/// Колесо автомобиля для тестов
/// </summary>
public record Wheel
{
    [JsonParserPropertyName("diameter")] 
    public double Diameter { get; init; }

    [JsonParserPropertyName("material")] 
    public string Material { get; init; }
}