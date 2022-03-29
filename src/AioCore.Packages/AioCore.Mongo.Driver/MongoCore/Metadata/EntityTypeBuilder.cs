using MongoDB.Driver;

namespace AioCore.Mongo.Driver.MongoCore.Metadata;

public class EntityTypeBuilder
{
    private readonly IMongoDatabase _database;
    
    public EntityTypeBuilder(IMongoDatabase database)
    {
        _database = database;
    }
}