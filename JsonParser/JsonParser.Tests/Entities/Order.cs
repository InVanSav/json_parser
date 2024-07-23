namespace JsonParser.JsonParser.Tests.Entities;

/// <summary>
/// Заказ для тестов
/// </summary>
public record Order
{
    [JsonParserPropertyName("orderId")]
    public string OrderId { get; init; }

    [JsonParserPropertyName("items")]
    public IReadOnlyCollection<OrderItem> Items { get; init; }

    [JsonParserPropertyName("totalAmount")]
    public double TotalAmount { get; init; }
}

/// <summary>
/// Элемент заказа для тестов
/// </summary>
public record OrderItem
{
    [JsonParserPropertyName("productName")]
    public string ProductName { get; init; }

    [JsonParserPropertyName("quantity")]
    public int Quantity { get; init; }

    [JsonParserPropertyName("price")]
    public double Price { get; init; }
}
