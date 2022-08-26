namespace AioCore.Shared.Extensions;

public static class FileExtension
{
    public static void WriteFile(this string path, string content)
    {
        using var fileStream = new FileStream(path, FileMode.OpenOrCreate,
            FileAccess.Write, FileShare.ReadWrite);
        using var streamWriter = new StreamWriter(fileStream);
        streamWriter.Write(content);
        streamWriter.Dispose();
    }

    public static void DeleteFile(this string path)
    {
        using var filestream = new FileStream(path, FileMode.Open, 
            FileAccess.Read, FileShare.ReadWrite);
        filestream.Close();
        File.Delete(path);
    }

    public static string ReadFile(this string path)
    {
        using var streamWriter = new StreamReader(path);
        var formatted = streamWriter.ReadToEnd();
        streamWriter.Dispose();
        return formatted;
    }

    public static byte[] ReadAsBytes(this string path)
    {
        return File.ReadAllBytes(path);
    }
}