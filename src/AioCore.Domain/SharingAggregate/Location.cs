using AioCore.Mongo;

namespace AioCore.Domain.SharingAggregate;

public class Location : MongoDocument
{
    public string RegionName { get; set; } = default!;
    
    public string ProvinceName { get; set; } = default!;
}