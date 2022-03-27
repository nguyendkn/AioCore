using AioCore.Mongo.OM.Attributes;

namespace AioCore.Mongo.OM.MongoCore;

public class MongoDocument
{
    [MongoKey] public Guid Id { get; set; }
}