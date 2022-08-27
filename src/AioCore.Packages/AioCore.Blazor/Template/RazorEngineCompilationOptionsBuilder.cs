using System.Reflection;
using Microsoft.CodeAnalysis;

namespace AioCore.Blazor.Template;

public class RazorEngineCompilationOptionsBuilder : IRazorEngineCompilationOptionsBuilder
{
    public RazorEngineCompilationOptions Options { get; set; }

    public RazorEngineCompilationOptionsBuilder(RazorEngineCompilationOptions? options = null)
    {
        Options = options ?? new RazorEngineCompilationOptions();
    }

    public void AddAssemblyReferenceByName(string assemblyName)
    {
        var assembly = Assembly.Load(new AssemblyName(assemblyName));
        this.AddAssemblyReference(assembly);
    }

    public void AddAssemblyReference(Assembly assembly)
    {
        this.Options.ReferencedAssemblies.Add(assembly);
    }

    public void AddAssemblyReference(Type type)
    {
        this.AddAssemblyReference(type.Assembly);

        foreach (var argumentType in type.GenericTypeArguments)
        {
            this.AddAssemblyReference(argumentType);
        }
    }

    public void AddMetadataReference(MetadataReference reference)
    {
        this.Options.MetadataReferences.Add(reference);
    }

    public void AddUsing(string namespaceName)
    {
        this.Options.DefaultUsing.Add(namespaceName);
    }

    public void Inherits(Type type)
    {
        this.Options.Inherits = this.RenderTypeName(type);
        this.AddAssemblyReference(type);
    }

    private string RenderTypeName(Type type)
    {
        IList<string?> elements = new List<string?>()
        {
            type.Namespace,
            RenderDeclaringType(type.DeclaringType),
            type.Name
        };

        var result = string.Join(".", elements.Where(e => !string.IsNullOrWhiteSpace(e)));

        if (result.Contains('`'))
        {
            result = result[..result.IndexOf("`", StringComparison.Ordinal)];
        }

        if (type.GenericTypeArguments.Length == 0)
        {
            return result;
        }

        return result + "<" + string.Join(",", type.GenericTypeArguments.Select(this.RenderTypeName)) + ">";
    }

    private string? RenderDeclaringType(Type? type)
    {
        if (type == null)
        {
            return null;
        }

        var parent = RenderDeclaringType(type.DeclaringType);

        if (string.IsNullOrWhiteSpace(parent))
        {
            return type.Name;
        }

        return parent + "." + type.Name;
    }
}