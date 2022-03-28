namespace AioCore.Mongo.Driver.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class MongoLocalFieldAttribute : Attribute
{
    public string LocalField { get; set; }

    public MongoLocalFieldAttribute(string localField)
    {
        LocalField = localField;
    }
}