using AioCore.Mongo.Driver.MongoCore.Abstracts;
using MongoDB.Driver;

namespace AioCore.Mongo.Driver.MongoCore;

public class MongoContext : IMongoContext
{
    private readonly IMongoContextBuilder _builder;
    public IMongoDatabase Database => _builder.Database;

    protected MongoContext(IMongoContextBuilder builder)
    {
        _builder = builder;
        builder.OnConfiguring(this);
    }

    public virtual void OnModelCreating()
    {
    }
}