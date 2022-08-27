using System.Reflection;

namespace AioCore.Blazor.Template;

public class RazorEngineCompiledTemplate : IRazorEngineCompiledTemplate
{
    private readonly MemoryStream _assemblyByteCode;
    private readonly Type _templateType;

    internal RazorEngineCompiledTemplate(MemoryStream assemblyByteCode, string templateNamespace)
    {
        this._assemblyByteCode = assemblyByteCode;

        var assembly = Assembly.Load(assemblyByteCode.ToArray());
        _templateType = assembly.GetType(templateNamespace + ".Template")!;
    }

    public static IRazorEngineCompiledTemplate LoadFromFile(string fileName,
        string templateNamespace = "TemplateNamespace")
    {
        return LoadFromFileAsync(fileName, templateNamespace).GetAwaiter().GetResult();
    }

    public static async Task<IRazorEngineCompiledTemplate> LoadFromFileAsync(string fileName,
        string templateNamespace = "TemplateNamespace")
    {
        var memoryStream = new MemoryStream();

        await using (var fileStream = new FileStream(
                         path: fileName,
                         mode: FileMode.Open,
                         access: FileAccess.Read,
                         share: FileShare.None,
                         bufferSize: 4096,
                         useAsync: true))
        {
            await fileStream.CopyToAsync(memoryStream);
        }

        return new RazorEngineCompiledTemplate(memoryStream, templateNamespace);
    }

    public static IRazorEngineCompiledTemplate LoadFromStream(Stream stream)
    {
        return LoadFromStreamAsync(stream).GetAwaiter().GetResult();
    }

    public static async Task<IRazorEngineCompiledTemplate> LoadFromStreamAsync(Stream stream,
        string templateNamespace = "TemplateNamespace")
    {
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        return new RazorEngineCompiledTemplate(memoryStream, templateNamespace);
    }

    public void SaveToStream(Stream stream)
    {
        SaveToStreamAsync(stream).GetAwaiter().GetResult();
    }

    public Task SaveToStreamAsync(Stream stream)
    {
        return _assemblyByteCode.CopyToAsync(stream);
    }

    public void SaveToFile(string fileName)
    {
        SaveToFileAsync(fileName).GetAwaiter().GetResult();
    }

    public Task SaveToFileAsync(string fileName)
    {
        using var fileStream = new FileStream(
            path: fileName,
            mode: FileMode.OpenOrCreate,
            access: FileAccess.Write,
            share: FileShare.None,
            bufferSize: 4096,
            useAsync: true);
        return _assemblyByteCode.CopyToAsync(fileStream);
    }

    public string Run(object? model = null)
    {
        return RunAsync(model).GetAwaiter().GetResult();
    }

    public async Task<string> RunAsync(object? model = null)
    {
        if (model != null && model.IsAnonymous())
        {
            model = new AnonymousTypeWrapper(model);
        }

        var instance = (IRazorEngineTemplate)Activator.CreateInstance(_templateType)!;
        if (model != null) instance.Model = model;

        await instance.ExecuteAsync();

        return await instance.ResultAsync();
    }
}