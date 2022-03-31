using AioCore.Mongo.Driver;
using AioCore.Mongo.Driver.Abstracts;
using MongoDB.Driver;
using Shared.Objects.AggregateModels.CategoryAggregate;
using Shared.Objects.AggregateModels.PageAggregate;

namespace Shared.Objects;

public class AioCoreContext : MongoContext
{
    public AioCoreContext(IMongoContextBuilder modelBuilder) : base(modelBuilder)
    {
    }

    public MongoSet<Category> Categories { get; set; } = default!;
    
    public MongoSet<Page> Pages { get; set; } = default!;

    protected override void OnModelCreating()
    {
        ModelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(
                new CreateIndexModel<Category>(Builders<Category>.IndexKeys.Descending(x => x.Timestamp)),
                new CreateIndexModel<Category>(Builders<Category>.IndexKeys.Text(x => x.Name))
            );
        });
        ModelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(
                new CreateIndexModel<Page>(Builders<Page>.IndexKeys.Ascending(x => x.Title)),
                new CreateIndexModel<Page>(Builders<Page>.IndexKeys.Hashed(x => x.Slug)),
                new CreateIndexModel<Page>(Builders<Page>.IndexKeys.Descending(x => x.Timestamp))
            );
        });
        base.OnModelCreating();
    }
}