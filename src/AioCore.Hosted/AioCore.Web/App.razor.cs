using System.Reflection;

namespace AioCore.Web;

public partial class App
{
    private readonly List<Assembly> _assemblies = new();

    protected override void OnInitialized()
    {
        _assemblies.AddRange(new[]
        {
            typeof(Plugin.Pages.Assembly).Assembly
        });
        base.OnInitialized();
    }
}