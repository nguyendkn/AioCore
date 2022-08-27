using System.Runtime.Serialization;

namespace AioCore.Blazor.Template;

public class RazorEngineException : Exception
{
    public RazorEngineException()
    {
    }

    protected RazorEngineException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public RazorEngineException(string message) : base(message)
    {
    }

    public RazorEngineException(string message, Exception innerException) : base(message, innerException)
    {
    }
}