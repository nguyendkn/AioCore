namespace AioCore.Elasticsearch.Tests;

public class User : EsDocument
{
    public string LastName { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string MiddleName { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string City { get; set; } = default!;
}