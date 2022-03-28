using AioCore.Mongo.Driver.Attributes;

namespace AioCore.Mongo.Driver.MongoCore;

public class MongoDocument
{
    [MongoKey] public Guid Id { get; set; }
}