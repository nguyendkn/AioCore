using System.Reflection;

namespace AioCore.Migrations;

public class Assembly
{
    public static readonly string? Name = typeof(Assembly).GetTypeInfo().Assembly.GetName().Name;
}