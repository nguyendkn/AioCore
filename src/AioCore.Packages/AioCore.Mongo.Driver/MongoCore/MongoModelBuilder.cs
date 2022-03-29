using AioCore.Mongo.Driver.MongoCore.Metadata;

namespace AioCore.Mongo.Driver.MongoCore;

public class MongoModelBuilder
{
    public virtual void Entity<TEntity>(Action<EntityTypeBuilder> buildAction) where TEntity : MongoDocument
    {
    }
}