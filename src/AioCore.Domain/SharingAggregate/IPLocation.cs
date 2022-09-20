using AioCore.Mongo;

namespace AioCore.Domain.SharingAggregate;

public class IPLocation : MongoDocument
{

    public long LocationId { get; set; }

    public int CountryId { get; set; }

    public int ProvinceId { get; set; }

    public string Code { get; set; }
}