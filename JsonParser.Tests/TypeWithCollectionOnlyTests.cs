using FluentAssertions;
using JsonParser.Tests.Entities;
using Xunit;

namespace JsonParser.Tests;

public class TypeWithCollectionOnlyTests
{
    [Fact]
    public void Deserialize_ShouldParseValidJson_ForOrder()
    {
        var json = "{\"orderId\": \"ORD123\", \"items\": [{\"productName\": \"Product1\", \"quantity\": 2, \"price\": 10.0}], \"totalAmount\": 20.0}";

        var result = CustomJsonParser.Deserialize<Order>(json);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(new Order
        {
            OrderId = "ORD123",
            Items = [
                new OrderItem
                {
                    ProductName = "Product1",
                    Quantity = 2,
                    Price = 10.0
                }
            ],
            TotalAmount = 20.0
        });
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidQuantity()
    {
        var json =
            "{\"orderId\": \"ORD123\", \"items\": [{\"productName\": \"Product1\", \"quantity\": \"invalid\", \"price\": 10.0}], \"totalAmount\": 20.0}";

        var result = CustomJsonParser.Deserialize<Order>(json);

        result.IsSuccess.Should().BeFalse();
        Assert.Contains("Свойство Quantity: The requested operation requires an element of type 'Number', but the target element has type 'String'.", result.ErrorMessages!);
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForMissingTotalAmount()
    {
        var json = "{\"orderId\": \"ORD123\", \"items\": [{\"productName\": \"Product1\", \"quantity\": 2, \"price\": 10.0}]}";

        var result = CustomJsonParser.Deserialize<Order>(json);

        result.IsSuccess.Should().BeFalse();
        Assert.Contains("Свойство TotalAmount: не найдено в JSON.", result.ErrorMessages!);
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidOrderId()
    {
        var json = "{\"orderId\": 12345, \"items\": [{\"productName\": \"Product1\", \"quantity\": 2, \"price\": 10.0}], \"totalAmount\": 20.0}";

        var result = CustomJsonParser.Deserialize<Order>(json);

        result.IsSuccess.Should().BeFalse();
        Assert.Contains("Свойство OrderId: The requested operation requires an element of type 'String', but the target element has type 'Number'.", result.ErrorMessages!);
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForInvalidPrice()
    {
        var json =
            "{\"orderId\": \"ORD123\", \"items\": [{\"productName\": \"Product1\", \"quantity\": 2, \"price\": \"invalid\"}], \"totalAmount\": 20.0}";

        var result = CustomJsonParser.Deserialize<Order>(json);

        result.IsSuccess.Should().BeFalse();
        Assert.Contains("Свойство Price: The requested operation requires an element of type 'Number', but the target element has type 'String'.", result.ErrorMessages!);
    }

    [Fact]
    public void Deserialize_ShouldReturnError_ForMissingItems()
    {
        var json = "{\"orderId\": \"ORD123\", \"totalAmount\": 20.0}";

        var result = CustomJsonParser.Deserialize<Order>(json);

        result.IsSuccess.Should().BeFalse();
        Assert.Contains("Свойство Items: не найдено в JSON.", result.ErrorMessages!);
    }
}