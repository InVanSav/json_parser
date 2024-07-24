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
        Assert.Contains("Свойство Age: The requested operation requires an element of type 'Number', but the target element has type 'String'.", result.ErrorMessages!);
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidPhoneNumber()
    {
        var json =
            "{\"name\": \"Иван Иванов\", \"age\": 25, \"phoneNumber\": \"invalid phone\", \"address\": {\"street\": \"Ленина\", \"city\": \"Москва\", \"zipCode\": \"101000\"}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        Assert.Contains("Свойство PhoneNumber: Exception has been thrown by the target of an invocation.", result.ErrorMessages!);
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForMissingAddress()
    {
        var json = "{\"name\": \"Иван Иванов\", \"age\": 25, \"phoneNumber\": {\"phoneNumberText\" :\"+7 999 123-45-67\"}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        Assert.Contains("Свойство Address: не найдено в JSON.", result.ErrorMessages!);
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidZipCode()
    {
        var json =
            "{\"name\": \"Иван Иванов\", \"age\": 25, \"phoneNumber\": {\"phoneNumberText\" :\"+7 999 123-45-67\"}, \"address\": {\"street\": \"Ленина\", \"city\": \"Москва\", \"zipCode\": 101000}}";

        var result = CustomJsonParser.Deserialize<User>(json);

        result.IsSuccess.Should().BeFalse();
        Assert.Contains("Свойство ZipCode: The requested operation requires an element of type 'String', but the target element has type 'Number'.", result.ErrorMessages!);
    }
}