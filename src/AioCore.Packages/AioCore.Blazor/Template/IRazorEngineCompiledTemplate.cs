namespace AioCore.Blazor.Template;

public interface IRazorEngineCompiledTemplate
{
    void SaveToStream(Stream stream);
    Task SaveToStreamAsync(Stream stream);
    void SaveToFile(string fileName);
    Task SaveToFileAsync(string fileName);
    string Run(object? model = null);
    Task<string> RunAsync(object? model = null);
}