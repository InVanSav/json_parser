namespace JsonParser;

/// <summary>
/// Класс-обертка для возвращения результата выполнения метода
/// </summary>
/// <typeparam name="T">Тип возвращаемых данных, если метод завершится успехом</typeparam>
public class JsonParsingResult<T>
{
    /// <summary>
    /// Свойство, указывающее на успешность операции
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Свойство для хранения сообщения об ошибке в случае неудачи
    /// </summary>
    public IReadOnlyCollection<string>? ErrorMessages { get; private set; }

    /// <summary>
    /// Дополнительные данные, которые могут быть возвращены в случае успеха
    /// </summary>
    public T Data { get; private set; }

    /// <summary>
    /// <inheritdoc cref="JsonParsingResult{T}"/>
    /// </summary>
    private JsonParsingResult(bool isSuccess, IReadOnlyCollection<string>? errorMessages = null, T data = default(T))
    {
        IsSuccess = isSuccess;
        ErrorMessages = errorMessages;
        Data = data;
    }

    /// <summary>
    /// Фабричный метод для создания успешного результата
    /// </summary>
    /// <param name="data">Данные, которые необходимо вернуть</param>
    public static JsonParsingResult<T> Success(T data = default(T))
    {
        return new JsonParsingResult<T>(true, null, data);
    }

    /// <summary>
    /// Фабричный метод для создания результата с ошибкой
    /// </summary>
    /// <param name="errorMessages">Сообщение ошибки</param>
    public static JsonParsingResult<T> Failure(IReadOnlyCollection<string>? errorMessages)
    {
        return new JsonParsingResult<T>(false, errorMessages);
    }
}
