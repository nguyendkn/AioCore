namespace AioCore.Blazor.Template;

public class TemplateServiceException: Exception
{
    public TemplateServiceException(string message) : base(message)
    {
    }

    public TemplateServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}