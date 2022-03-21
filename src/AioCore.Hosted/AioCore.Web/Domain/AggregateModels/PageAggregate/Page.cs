namespace AioCore.Web.Domain.AggregateModels.PageAggregate;

public class Page
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}