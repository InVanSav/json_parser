namespace JsonParser.Attributes;

/// <summary>
/// Кастомный аттрибут для сопоставления имени свойства при сериализации JSON
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class JsonParserPropertyNameAttribute : Attribute
{
    /// <summary>
    /// Название свойства
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// <inheritdoc cref="JsonParserPropertyNameAttribute"/>
    /// </summary>
    public JsonParserPropertyNameAttribute(string name)
    {
        Name = name;
    }
}