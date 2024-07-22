namespace JsonParser.JsonParser.Domain;

/// <summary>
/// Класс-обертка для возвращения результата выполнения метода
/// </summary>
/// <typeparam name="T">Тип возвращаемых данных, если метод завершится успехом</typeparam>
public class Result<T>
{
    /// <summary>
    /// Свойство, указывающее на успешность операции
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Свойство для хранения сообщения об ошибке в случае неудачи
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Дополнительные данные, которые могут быть возвращены в случае успеха
    /// </summary>
    public T Data { get; private set; }

    /// <summary>
    /// <inheritdoc cref="Result{T}"/>
    /// </summary>
    private Result(bool isSuccess, string? errorMessage = null, T data = default(T))
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Data = data;
    }

    /// <summary>
    /// Фабричный метод для создания успешного результата
    /// </summary>
    /// <param name="data">Данные, которые необходимо вернуть</param>
    public static Result<T> Success(T data = default(T))
    {
        return new Result<T>(true, null, data);
    }

    /// <summary>
    /// Фабричный метод для создания результата с ошибкой
    /// </summary>
    /// <param name="errorMessage">Сообщение ошибки</param>
    public static Result<T> Failure(string? errorMessage)
    {
        return new Result<T>(false, errorMessage);
    }
}

/// <summary>
/// Класс-обертка для возвращения результата выполнения метода
/// </summary>
public class Result
{
    /// <summary>
    /// Свойство, указывающее на успешность операции
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Свойство для хранения сообщения об ошибке в случае неудачи
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// <inheritdoc cref="Result"/>
    /// </summary>
    private Result(bool isSuccess, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Фабричный метод для создания успешного результата
    /// </summary>
    public static Result Success()
    {
        return new Result(true);
    }

    /// <summary>
    /// Фабричный метод для создания результата с ошибкой
    /// </summary>
    /// <param name="errorMessage">Сообщение ошибки</param>
    public static Result Failure(string? errorMessage)
    {
        return new Result(false, errorMessage);
    }
}
