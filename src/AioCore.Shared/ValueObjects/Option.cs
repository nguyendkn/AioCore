namespace AioCore.Shared.ValueObjects;

public class Option<T>
{
    public string Name { get; set; } = default!;

    public T Value { get; set; } = default!;
}

