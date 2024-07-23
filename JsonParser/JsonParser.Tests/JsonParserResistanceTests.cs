using FluentAssertions;
using JsonParser.Tests.Entities;
using Xunit;

namespace JsonParser.Tests;

public class JsonParserResistanceTests
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
}