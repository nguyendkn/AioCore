using System.Reflection.Metadata;
using System.Text;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AioCore.Blazor.Template;

public class RazorEngine : IRazorEngine
{
    public IRazorEngineCompiledTemplate Compile<T>(string content,
        Action<IRazorEngineCompilationOptionsBuilder>? builderAction = null) where T : IRazorEngineTemplate
    {
        IRazorEngineCompilationOptionsBuilder compilationOptionsBuilder = new RazorEngineCompilationOptionsBuilder();

        compilationOptionsBuilder.AddAssemblyReference(typeof(T).Assembly);
        compilationOptionsBuilder.Inherits(typeof(T));

        builderAction?.Invoke(compilationOptionsBuilder);

        var memoryStream = CreateAndCompileToStream(content, compilationOptionsBuilder.Options);

        return new RazorEngineCompiledTemplate(memoryStream, compilationOptionsBuilder.Options.TemplateNamespace);
    }

    public Task<IRazorEngineCompiledTemplate> CompileAsync<T>(string content,
        Action<IRazorEngineCompilationOptionsBuilder>? builderAction = null) where T : IRazorEngineTemplate
    {
        return Task.Factory.StartNew(() => Compile<T>(content: content, builderAction: builderAction));
    }

    public IRazorEngineCompiledTemplate Compile(string content,
        Action<IRazorEngineCompilationOptionsBuilder>? builderAction = null)
    {
        IRazorEngineCompilationOptionsBuilder compilationOptionsBuilder = new RazorEngineCompilationOptionsBuilder();
        compilationOptionsBuilder.Inherits(typeof(RazorEngineTemplateBase));

        builderAction?.Invoke(compilationOptionsBuilder);

        var memoryStream = CreateAndCompileToStream(content, compilationOptionsBuilder.Options);

        return new RazorEngineCompiledTemplate(memoryStream, compilationOptionsBuilder.Options.TemplateNamespace);
    }

    public Task<IRazorEngineCompiledTemplate> CompileAsync(string content,
        Action<IRazorEngineCompilationOptionsBuilder>? builderAction = null)
    {
        return Task.Factory.StartNew(() => Compile(content: content, builderAction: builderAction));
    }

    protected virtual MemoryStream CreateAndCompileToStream(string templateSource,
        RazorEngineCompilationOptions options)
    {
        templateSource = WriteDirectives(templateSource, options);

        var engine = RazorProjectEngine.Create(
            RazorConfiguration.Default,
            RazorProjectFileSystem.Create(@"."),
            builder => { builder.SetNamespace(options.TemplateNamespace); });

        var fileName = string.IsNullOrWhiteSpace(options.TemplateFilename)
            ? Path.GetRandomFileName()
            : options.TemplateFilename;

        var document = RazorSourceDocument.Create(templateSource, fileName);

        var codeDocument = engine.Process(
            document,
            null,
            new List<RazorSourceDocument>(),
            new List<TagHelperDescriptor>());

        var razorCSharpDocument = codeDocument.GetCSharpDocument();

        var syntaxTree = CSharpSyntaxTree.ParseText(razorCSharpDocument.GeneratedCode);

        var compilation = CSharpCompilation.Create(
            fileName,
            new[]
            {
                syntaxTree
            },
            (options.ReferencedAssemblies ?? throw new InvalidOperationException())
                .Select(ass =>
                {
#if NETSTANDARD2_0
                            return  MetadataReference.CreateFromFile(ass.Location);
#else
                    unsafe
                    {
                        ass.TryGetRawMetadata(out var blob, out var length);
                        var moduleMetadata = ModuleMetadata.CreateFromMetadata((IntPtr)blob, length);
                        var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                        var metadataReference = assemblyMetadata.GetReference();

                        return metadataReference;
                    }
#endif
                })
                .Concat(options.MetadataReferences)
                .ToList(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var memoryStream = new MemoryStream();

        var emitResult = compilation.Emit(memoryStream);

        if (!emitResult.Success)
        {
            var exception = new RazorEngineCompilationException
            {
                Errors = emitResult.Diagnostics.ToList(),
                GeneratedCode = razorCSharpDocument.GeneratedCode
            };

            throw exception;
        }

        memoryStream.Position = 0;

        return memoryStream;
    }

    protected virtual string WriteDirectives(string content, RazorEngineCompilationOptions options)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"@inherits {options.Inherits}");

        foreach (var entry in options.DefaultUsing)
        {
            stringBuilder.AppendLine($"@using {entry}");
        }

        stringBuilder.Append(content);

        return stringBuilder.ToString();
    }
}