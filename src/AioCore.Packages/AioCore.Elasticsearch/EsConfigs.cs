namespace AioCore.Elasticsearch;

public class EsConfigs
{
    public string Url { get; set; } = default!;
    public string Index { get; set; } = default!;
    public string? UserName { get; set; }
    public string? Password { get; set; }
}