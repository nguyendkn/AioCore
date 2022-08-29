namespace AioCore.Shared.Extensions;

public static class ObjectExtensions
{
    public static T ConvertTo<T>(this object value)
    {
        return (T)Convert.ChangeType(value, typeof(T));
    }

    public static bool TryConvertTo<T>(this object value, out T? convertedValue)
    {
        try
        {
            convertedValue = (T)Convert.ChangeType(value, typeof(T));
            return true;
        }
        catch
        {
            convertedValue = default;
            return false;
        }
    }
}