namespace JsonParser;

public static class CustomJsonParser
{
    public static JsonParsingResult<T> Deserialize<T>(T deserializationObject)
    {
        return JsonParsingResult<T>.Failure(Array.Empty<string>());
    }
}