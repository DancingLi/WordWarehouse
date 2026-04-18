namespace WordWarehouse.ViewModels;

public class EnumOption<T> where T : struct, Enum
{
    public string Label { get; set; } = string.Empty;

    public T? Value { get; set; }
}
