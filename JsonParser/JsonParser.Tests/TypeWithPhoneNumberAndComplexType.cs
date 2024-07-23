using FluentAssertions;
using JsonParser.Tests.Entities;
using Xunit;

namespace JsonParser.Tests;

public class TypeWithPhoneNumberAndComplexType
{
    [Fact]
    public void Deserialize_ShouldParseValidJson_ForUser()
    {
        var json =
            "{\"name\": \"Иван Иванов\", \"age\": 25, \"phoneNumber\": {\"phoneNumberText\" :\"+7 999 123-45-67\"}, \"address\": {\"street\": \"Ленина\", \"city\": \"Москва\", \"zipCode\": \"101000\"}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(new User
        {
            Name = "Иван Иванов",
            Age = 25,
            PhoneNumber = new PhoneNumber("79991234567"),
            Address = new Address
            {
                Street = "Ленина",
                City = "Москва",
                ZipCode = "101000"
            }
        });
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidAge()
    {
        var json =
            "{\"name\": \"Иван Иванов\", \"age\": \"invalid\", \"phoneNumber\": {\"phoneNumberText\" :\"+7 999 123-45-67\"}, \"address\": {\"street\": \"Ленина\", \"city\": \"Москва\", \"zipCode\": \"101000\"}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Свойство Age: не является целочисленным.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidPhoneNumber()
    {
        var json =
            "{\"name\": \"Иван Иванов\", \"age\": 25, \"phoneNumber\": \"invalid phone\", \"address\": {\"street\": \"Ленина\", \"city\": \"Москва\", \"zipCode\": \"101000\"}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Свойство PhoneNumberText: не является валидным номером телефона.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForMissingAddress()
    {
        var json = "{\"name\": \"Иван Иванов\", \"age\": 25, \"phoneNumber\": {\"phoneNumberText\" :\"+7 999 123-45-67\"}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().Contain("Свойство Address: не найдено в JSON.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidZipCode()
    {
        var json =
            "{\"name\": \"Иван Иванов\", \"age\": 25, \"phoneNumber\": {\"phoneNumberText\" :\"+7 999 123-45-67\"}, \"address\": {\"street\": \"Ленина\", \"city\": \"Москва\", \"zipCode\": 101000}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Свойство ZipCode: не является строкой.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForMissingCityInAddress()
    {
        var json =
            "{\"name\": \"Иван Иванов\", \"age\": 25, \"phoneNumber\": {\"phoneNumberText\" :\"+7 999 123-45-67\"}, \"address\": {\"street\": \"Ленина\", \"zipCode\": \"101000\"}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().Contain("Свойство City: не найдено в JSON.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForMissingPhoneNumber()
    {
        var json =
            "{\"name\": \"Иван Иванов\", \"age\": 25, \"address\": {\"street\": \"Ленина\", \"city\": \"Москва\", \"zipCode\": \"101000\"}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Свойство PhoneNumber: не найдено в JSON.");
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidNestedObject()
    {
        var json =
            "{\"name\": \"John Doe\", \"age\": 30, \"phoneNumber\": {\"phoneNumberText\" :\"+79991234567\"}, \"address\": {\"street\": \"Main St\", \"city\": 123, \"zipCode\": \"12345\"}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Свойство City: не является строкой.");
    }
}