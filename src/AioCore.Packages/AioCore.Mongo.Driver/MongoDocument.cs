using AioCore.Mongo.Driver.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace AioCore.Mongo.Driver;

public class MongoDocument
{
    [BsonId] [MongoKey] public Guid Id { get; set; } = Guid.NewGuid();
}