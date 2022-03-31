using AioCore.Mongo.Driver.Abstracts;
using MongoDB.Driver;

namespace AioCore.Mongo.Driver;

public class MongoContext
{
    protected readonly IMongoContextBuilder ModelBuilder;
    public IMongoDatabase Database => ModelBuilder.Database;

    protected MongoContext(IMongoContextBuilder modelBuilder)
    {
        ModelBuilder = modelBuilder;
        modelBuilder.OnConfiguring(this);
        modelBuilder.OnModelCreating(OnModelCreating);
    }

    protected virtual void OnModelCreating()
    {
    }
}