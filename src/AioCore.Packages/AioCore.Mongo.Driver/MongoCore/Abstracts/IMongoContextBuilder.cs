using MongoDB.Driver;

namespace AioCore.Mongo.Driver.MongoCore.Abstracts;

public interface IMongoContextBuilder
{
    IMongoDatabase Database { get; }

    void OnConfiguring(MongoContext context);
    
    void OnModelCreating();
}