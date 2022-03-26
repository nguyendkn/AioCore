using System.Reflection;

namespace AioCore.Pages;

public static class Assemblies
{
    public static IEnumerable<Assembly> Load => new[]
    {
        typeof(Program).Assembly,
        typeof(Shared.Objects.Assembly).Assembly
    };
}