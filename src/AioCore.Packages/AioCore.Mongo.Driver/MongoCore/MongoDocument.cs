using AioCore.Mongo.Driver.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace AioCore.Mongo.Driver.MongoCore;

public class MongoDocument
{
    [BsonId] [MongoKey] public Guid Id { get; set; }
}