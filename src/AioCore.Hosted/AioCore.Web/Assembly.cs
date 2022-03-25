using System.Reflection;

namespace AioCore.Web;

public static class Assemblies
{
    public static IEnumerable<Assembly> Load => new[]
    {
        typeof(Program).Assembly,
        typeof(Shared.Objects.Assembly).Assembly,
        typeof(Feature.Pages.Assembly).Assembly
    };
}