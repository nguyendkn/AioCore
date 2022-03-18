namespace Shared.Extensions;

public static class FileExtension
{
    public static void WriteFile(this string path, string content)
    {
        using var streamWriter = new StreamWriter(path);
        streamWriter.Write(content);
        streamWriter.Dispose();
    }

    public static string ReadFile(this string path)
    {
        using var streamWriter = new StreamReader(path);
        var formatted = streamWriter.ReadToEnd();
        streamWriter.Dispose();
        return formatted;
    }
}