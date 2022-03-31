using System;
using AioCore.Mongo.Driver.Abstracts;
using MongoDB.Driver;

namespace AioCore.Mongo.Driver.Tests;

public class AioCoreContext : MongoContext, IDisposable
{
    public AioCoreContext(IMongoContextBuilder modelBuilder) : base(modelBuilder)
    {
    }

    public MongoSet<Province> Provinces { get; set; } = default!;

    protected override void OnModelCreating()
    {
        ModelBuilder.Entity<Province>(entity =>
        {
            entity.HasIndex(
                new CreateIndexModel<Province>(Builders<Province>.IndexKeys.Text(x => x.Name)),
                new CreateIndexModel<Province>(Builders<Province>.IndexKeys.Descending(x => x.Timestamp))
            );
        });
        base.OnModelCreating();
    }

    public void Dispose()
    {
    }
}