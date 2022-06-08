using MongoDB.Driver;

namespace Package.Mongo;

public class MongoContext
{
    private IMongoDatabase? _mongoDatabase;
    private readonly string _connectionString;
    private readonly string? _databaseName;

    public MongoContext(string connectionString, string? name)
    {
        _connectionString = connectionString;
        _databaseName = name;

        MongoConfiguring();
    }

    private void MongoConfiguring()
    {
        var mongoClient = new MongoClient(_connectionString);
        _mongoDatabase = mongoClient.GetDatabase(_databaseName);
    }
}