using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace AioCore.Elasticsearch.Tests;

public class ElasticsearchFixture
{
    public ElasticsearchFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddElasticsearchContext<AioCoreContext>(new EsConfigs
        {
            Url = "https://localhost:9200",
            Index = "test",
            UserName = "elastic",
            Password = "lOsxTsJlHJNdviyIPEEu"
        });

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public ServiceProvider ServiceProvider { get; private set; }
}

public class ElasticsearchTests : IClassFixture<ElasticsearchFixture>
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _testOutputHelper;

    public ElasticsearchTests(ElasticsearchFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _serviceProvider = fixture.ServiceProvider;
    }

    [Fact]
    public async Task TestInsert()
    {
        using var context = _serviceProvider.GetRequiredService<AioCoreContext>();
        await context.Users.AddAsync(new User
        {
            LastName = Faker.Name.Last(),
            FirstName = Faker.Name.First(),
            MiddleName = Faker.Name.Middle(),
            Address = Faker.Address.Country(),
            City = Faker.Address.City(),
        });
    }
}