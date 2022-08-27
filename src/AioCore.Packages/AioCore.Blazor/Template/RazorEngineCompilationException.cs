using System.Runtime.Serialization;
using Microsoft.CodeAnalysis;

namespace AioCore.Blazor.Template;

public class RazorEngineCompilationException : RazorEngineException
{
    public RazorEngineCompilationException()
    {
    }

    protected RazorEngineCompilationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public RazorEngineCompilationException(Exception innerException) : base(null, innerException)
    {
    }

    public List<Diagnostic>? Errors { get; set; }

    public string? GeneratedCode { get; set; }

    public override string Message
    {
        get
        {
            var errors = string.Join("\n",
                Errors?.Where(w => w.IsWarningAsError || w.Severity == DiagnosticSeverity.Error) ??
                Array.Empty<Diagnostic>());
            return "Unable to compile template: " + errors;
        }
    }
}