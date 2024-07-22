namespace JsonParser.JsonParser.Domain;

/// <summary>
/// Класс-обертка для возвращения результата выполнения метода
/// </summary>
/// <typeparam name="T">Тип возвращаемых данных, если метод завершится успехом</typeparam>
public class JsonSerializationResult<T>
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
    /// <inheritdoc cref="JsonSerializationResult{T}"/>
    /// </summary>
    private JsonSerializationResult(bool isSuccess, IReadOnlyCollection<string>? errorMessages = null, T data = default(T))
    {
        IsSuccess = isSuccess;
        ErrorMessages = errorMessages;
        Data = data;
    }

    /// <summary>
    /// Фабричный метод для создания успешного результата
    /// </summary>
    /// <param name="data">Данные, которые необходимо вернуть</param>
    public static JsonSerializationResult<T> Success(T data = default(T))
    {
        return new JsonSerializationResult<T>(true, null, data);
    }

    /// <summary>
    /// Фабричный метод для создания результата с ошибкой
    /// </summary>
    /// <param name="errorMessages">Сообщение ошибки</param>
    public static JsonSerializationResult<T> Failure(IReadOnlyCollection<string>? errorMessages)
    {
        return new JsonSerializationResult<T>(false, errorMessages);
    }
}
