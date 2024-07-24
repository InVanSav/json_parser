using FluentAssertions;
using JsonParser.Tests.Entities;
using Xunit;

namespace JsonParser.Tests;

public class JsonParserCommonTests
{
    [Fact]
    public void Deserialize_ShouldReturnError_ForNullJsonString()
    {
        string? json = null;

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("JSON строка не может быть null.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidJsonString()
    {
        var json = "{invalidJson}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Ошибка парсинга JSON строки.");
    }

    [Fact]
    public void Deserialize_ShouldReturnErrors_ForTypeMismatch()
    {
        var json = "{\"address\":123,\"rooms\":[],\"floors\":\"Not an array\",\"price\":\"Not a number\"}";

        var result = CustomJsonParser.Deserialize<House>(json);

        result.IsSuccess.Should().BeFalse();
        Assert.Contains("Свойство Address: The requested operation requires an element of type 'String', but the target element has type 'Number'.", result.ErrorMessages);
        Assert.Contains("Ожидался массив в JSON, но был найден другой тип.", result.ErrorMessages);
        Assert.Contains("Свойство Price: The requested operation requires an element of type 'Number', but the target element has type 'String'.", result.ErrorMessages);;
    }
}