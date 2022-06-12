using MongoDB.Driver;

namespace Package.Mongo;

public interface IMongoContextBuilder
{
    IMongoDatabase Database { get; }

    void Entity<TEntity>(Action<EntityTypeBuilder<TEntity>> action) where TEntity : class;

    void OnConfiguring(MongoContext context);

    void OnModelCreating(Action action);
}