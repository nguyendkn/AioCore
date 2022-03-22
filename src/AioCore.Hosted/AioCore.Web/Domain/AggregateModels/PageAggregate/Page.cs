using AioCore.Redis.OM.Modeling;

namespace AioCore.Web.Domain.AggregateModels.PageAggregate;

[Document(StorageType = StorageType.Json)]
public class Page
{
    [RedisIdField] public string Id { get; set; } = default!;

    [Searchable(Sortable = true)] public string Name { get; set; }

    [Indexed] public string? Description { get; set; }

    public Page(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}