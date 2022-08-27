using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;

namespace AioCore.Blazor.Template;

public class RazorEngineCompilationOptions
{
    public HashSet<Assembly>? ReferencedAssemblies { get; set; }

    public HashSet<MetadataReference> MetadataReferences { get; set; } = new HashSet<MetadataReference>();
    public string TemplateNamespace { get; set; } = "TemplateNamespace";
    public string TemplateFilename { get; set; } = "";
    public string Inherits { get; set; } = "RazorEngineCore.RazorEngineTemplateBase";

    public HashSet<string> DefaultUsing { get; set; } = new()
    {
        "System.Linq",
        "System.Collections",
        "System.Collections.Generic"
    };

    public RazorEngineCompilationOptions()
    {
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        var isFullFramework =
            RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);

        if (isWindows && isFullFramework)
        {
            ReferencedAssemblies = new HashSet<Assembly>
            {
                typeof(object).Assembly,
                Assembly.Load(new AssemblyName(
                    "Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")),
                typeof(RazorEngineTemplateBase).Assembly,
                typeof(GCSettings).Assembly,
                typeof(IList).Assembly,
                typeof(IEnumerable<>).Assembly,
                typeof(Enumerable).Assembly,
                typeof(Expression).Assembly,
                Assembly.Load(
                    new AssemblyName("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"))
            };
        }

        if (isWindows && !isFullFramework) // i.e. NETCore
        {
            ReferencedAssemblies = new HashSet<Assembly>
            {
                typeof(object).Assembly,
                Assembly.Load(new AssemblyName("Microsoft.CSharp")),
                typeof(RazorEngineTemplateBase).Assembly,
                Assembly.Load(new AssemblyName("System.Runtime")),
                typeof(IList).Assembly,
                typeof(IEnumerable<>).Assembly,
                Assembly.Load(new AssemblyName("System.Linq")),
                Assembly.Load(new AssemblyName("System.Linq.Expressions")),
                Assembly.Load(new AssemblyName("netstandard"))
            };
        }

        if (!isWindows)
        {
            ReferencedAssemblies = new HashSet<Assembly>
            {
                typeof(object).Assembly,
                Assembly.Load(new AssemblyName("Microsoft.CSharp")),
                typeof(RazorEngineTemplateBase).Assembly,
                Assembly.Load(new AssemblyName("System.Runtime")),
                typeof(IList).Assembly,
                typeof(IEnumerable<>).Assembly,
                Assembly.Load(new AssemblyName("System.Linq")),
                Assembly.Load(new AssemblyName("System.Linq.Expressions")),
                Assembly.Load(new AssemblyName("netstandard"))
            };
        }
    }
}