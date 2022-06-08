using System.Reflection;

namespace AioCore.Web;

public static class Assemblies
{
    public static Assembly[] All => new[]
    {
        typeof(Settings.Cache.Assembly).Assembly,
        typeof(Settings.Layout.Assembly).Assembly,
        typeof(Settings.Seo.Assembly).Assembly,
        typeof(Settings.Setup.Assembly).Assembly,
        typeof(Settings.Slug.Assembly).Assembly
    };
}