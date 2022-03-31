using System;
using Shared.Extensions;

namespace AioCore.Mongo.Driver.Tests;

public class AioCoreDocument : MongoDocument
{
    public string? Timestamp { get; set; }

    public DateTimeOffset TimestampOffset { get; set; }

    public AioCoreDocument()
    {
        Id = Guid.NewGuid();
        TimestampOffset = DateTimeOffset.Now;
        Timestamp = TimestampOffset.ToHexCode();
    }
}