using System.Reflection;
using System.Text.Json;

namespace JsonParser.Types;

/// <summary>
/// Абстракция типа, который можно спарсить из JSON с помощью <see cref="CustomJsonParser"/>
/// </summary>
public abstract class ParsableType
{
    /// <summary>
    /// Проверяет возможность спарсить переданный в свойствах тип
    /// </summary>
    public abstract bool CanParse(PropertyInfo prop);

    /// <summary>
    /// Выполнить парсинг типа из JSON
    /// </summary>
    public abstract dynamic? Parse(PropertyInfo prop, JsonElement jsonProp, List<string> errors);
}