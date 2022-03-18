namespace Shared.Objects;

public class Page
{
    public string Title { get; set; } = default!;

    public string Timestamp { get; set; } = default!;

    public string Slug { get; set; } = default!;

    public string? Template { get; set; }

    public List<string>? Includes { get; set; }
}