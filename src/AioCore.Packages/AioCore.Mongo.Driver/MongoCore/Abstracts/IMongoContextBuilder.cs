using AioCore.Mongo.Driver.MongoCore.Metadata;
using MongoDB.Driver;

namespace AioCore.Mongo.Driver.MongoCore.Abstracts;

public interface IMongoContextBuilder
{
    IMongoDatabase Database { get; }

    void Entity<TEntity>(Action<EntityTypeBuilder<TEntity>> action) where TEntity : class;

    void OnConfiguring(MongoContext context);

    void OnModelCreating(Action action);
}