using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shared.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace AioCore.Mongo.Driver.Tests;

public class MongoQueryFixture
{
    public MongoQueryFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddMongoContext<AioCoreContext>("mongodb://localhost:27017", "aiocore");

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public ServiceProvider ServiceProvider { get; private set; }
}

public class MongoQueryTests : IClassFixture<MongoQueryFixture>
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _testOutputHelper;

    public MongoQueryTests(MongoQueryFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _serviceProvider = fixture.ServiceProvider;
    }

    [Fact]
    public async Task TestCreate()
    {
        using var context = _serviceProvider.GetRequiredService<AioCoreContext>();
        await context.Provinces.AddRangeAsync(new List<Province>
        {
            new() {Name = Faker.Country.Name()},
            new() {Name = Faker.Country.Name()},
            new() {Name = Faker.Country.Name()},
            new() {Name = Faker.Country.Name()},
            new() {Name = Faker.Country.Name()},
            new() {Name = Faker.Country.Name()},
            new() {Name = Faker.Country.Name()},
            new() {Name = Faker.Country.Name()},
            new() {Name = Faker.Country.Name()},
            new() {Name = Faker.Country.Name()}
        });
    }

    [Fact]
    public async Task TestList()
    {
        using var context = _serviceProvider.GetRequiredService<AioCoreContext>();
        var provinces = await context.Provinces.Where(x => true, "pakistan").ToListAsync();
        _testOutputHelper.WriteLine($"Provinces count: {provinces.Count}");
    }
}