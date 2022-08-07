using System.Reflection;

namespace AioCore.Web;

public class Assembly
{
    public static readonly string? Name = typeof(Assembly).GetTypeInfo().Assembly.GetName().Name;
}