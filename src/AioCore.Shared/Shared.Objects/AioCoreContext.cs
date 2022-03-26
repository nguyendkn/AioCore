﻿using AioCore.Mongo.OM.MongoCore;
using Shared.Objects.AggregateModels.PageAggregate;

namespace Shared.Objects;

public class AioCoreContext : MongoContext
{
    public MongoSet<Page> Pages { get; set; } = default!;
}