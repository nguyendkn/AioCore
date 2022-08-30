namespace AioCore.Database;

public class DbContextSchema : IDbContextSchema
{
    public string? Schema { get; }
    
    public DbContextSchema(string schema)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
    }
}