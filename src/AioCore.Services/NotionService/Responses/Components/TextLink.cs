namespace AioCore.Services.NotionService.Responses.Components;

public class TextLink
{
    public string Type { get; set; } = "url";
    public string Url { get; set; } = default!;
}