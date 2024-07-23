using FluentAssertions;
using JsonParser.Tests.Entities;
using Xunit;

namespace JsonParser.Tests;

public class JsonParserWithComplexCollectionAndSimpleCollection
{
    [Fact]
    public void Deserialize_ShouldParseValidJson_ForHouse()
    {
        var json =
            "{\"address\":\"Улица Ленина, 10\",\"rooms\":[{\"name\":\"Гостиная\",\"area\":35.5,\"windows\":3},{\"name\":\"Спальня\",\"area\":20.0,\"windows\":2}],\"floors\":[1,2],\"price\":5000000.0}";

        var result = CustomJsonParser.Deserialize<House>(json);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(new House
        {
            Address = "Улица Ленина, 10",
            Rooms = new[]
            {
                new Room { Name = "Гостиная", Area = 35.5, Windows = 3 },
                new Room { Name = "Спальня", Area = 20.0, Windows = 2 }
            },
            Floors = new[] { 1, 2 },
            Price = 5000000.0
        });
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidRoomArea()
    {
        var json =
            "{\"address\":\"Улица Ленина, 10\",\"rooms\":[{\"name\":\"Гостиная\",\"area\":\"invalid\",\"windows\":3}],\"floors\":[1,2],\"price\":5000000.0}";

        var result = CustomJsonParser.Deserialize<House>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Свойство Area: не является числом с плавающей запятой.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidFloor()
    {
        var json =
            "{\"address\":\"Улица Ленина, 10\",\"rooms\":[{\"name\":\"Гостиная\",\"area\":35.5,\"windows\":3}],\"floors\":[{\"invalid\",\"2\"}],\"price\":5000000.0}";

        var result = CustomJsonParser.Deserialize<House>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Свойство Floors: не является целочисленным.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForMissingAddress()
    {
        var json = "{\"rooms\":[{\"name\":\"Гостиная\",\"area\":35.5,\"windows\":3}],\"floors\":[1,2],\"price\":5000000.0}";

        var result = CustomJsonParser.Deserialize<House>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().Contain("Свойство Address: не найдено в JSON.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidRoomName()
    {
        var json = "{\"address\":\"Улица Ленина, 10\",\"rooms\":[{\"name\":123,\"area\":35.5,\"windows\":3}],\"floors\":[1,2],\"price\":5000000.0}";

        var result = CustomJsonParser.Deserialize<House>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Свойство Name: не является строкой.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidWindowsInRoom()
    {
        var json =
            "{\"address\":\"Улица Ленина, 10\",\"rooms\":[{\"name\":\"Гостиная\",\"area\":35.5,\"windows\":\"invalid\"}],\"floors\":[1,2],\"price\":5000000.0}";

        var result = CustomJsonParser.Deserialize<House>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Свойство Windows: не является целочисленным.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidPrice()
    {
        var json =
            "{\"address\":\"Улица Ленина, 10\",\"rooms\":[{\"name\":\"Гостиная\",\"area\":35.5,\"windows\":3}],\"floors\":[1,2],\"price\":\"invalid\"}";

        var result = CustomJsonParser.Deserialize<House>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Свойство Price: не является числом с плавающей запятой.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForMissingRooms()
    {
        var json = "{\"address\":\"Улица Ленина, 10\",\"floors\":[1,2],\"price\":5000000.0}";

        var result = CustomJsonParser.Deserialize<House>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().Contain("Свойство Rooms: не найдено в JSON.");
    }
}