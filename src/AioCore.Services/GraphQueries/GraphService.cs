using System.Text;
using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.DynamicAggregate;
using AioCore.Services.GraphQueries.FilterTypes;
using AioCore.Shared.Extensions;
using MongoDB.Driver;

namespace AioCore.Services.GraphQueries;

public interface IGraphService
{
    Task<List<DynamicEntity>> ExecuteAsync(GraphRequest request);
}

public class GraphService : IGraphService
{
    private readonly DynamicContext _dynamicContext;

    public GraphService(DynamicContext dynamicContext)
    {
        _dynamicContext = dynamicContext;
    }

    public async Task<List<DynamicEntity>> ExecuteAsync(GraphRequest request)
    {
        if (request.Filters == null) return default!;
        var builder = new StringBuilder();
        foreach (var filter in request.Filters)
        {
            var field = $"'Data.{filter.Key}'";
            switch (filter.Value)
            {
                case EqualFilterType equalFilterType:
                    var equalQuery = "{" + field + ": {$eq: '" + equalFilterType.Value + "'}}";
                    builder.AppendLine(equalQuery);
                    break;
                case GatherThanFilterType gatherThanFilterType:
                    var gatherThanQuery = "{" + field + ": {$gt: '" + gatherThanFilterType.Value + "'}}";
                    builder.AppendLine(gatherThanQuery);
                    break;
                case GreaterThanOrEqualFilterType gatherThanFilterType:
                    var gatherThanOrEqualQuery = "{" + field + ": {$gte: '" + gatherThanFilterType.Value + "'}}";
                    builder.AppendLine(gatherThanOrEqualQuery);
                    break;
                case InFilterType inFilterType:
                    var inQuery = "{" + field + ": {$in: '" + inFilterType.Arguments.Serialize() + "'}}";
                    builder.AppendLine(inQuery);
                    break;
                case LessThanFilterType lessThanFilterType:
                    var lessThanQuery = "{" + field + ": {$lt: '" + lessThanFilterType.Value + "'}}";
                    builder.AppendLine(lessThanQuery);
                    break;
                case LessThanOrEqualFilterType lessThanOrEqualFilterType:
                    var lessThanOrEqualQuery = "{" + field + ": {$lte: '" + lessThanOrEqualFilterType.Value + "'}}";
                    builder.AppendLine(lessThanOrEqualQuery);
                    break;
                case NotEqualFilterType notEqualFilterType:
                    var notEqualQuery = "{" + field + ": {$ne: '" + notEqualFilterType.Value + "'}}";
                    builder.AppendLine(notEqualQuery);
                    break;
                case NotInFilterType notInFilterType:
                    var valueBuilder = notInFilterType.Arguments.Select(x => $"'{x}'").Serialize();
                    var notInQuery = "{" + field + ": {$nin: [" + valueBuilder + "}}";
                    builder.AppendLine(notInQuery);
                    break;
            }
        }

        var queryBuilder = builder.ToString();
        var fluent = _dynamicContext.Entities.Collection(request.Collection).Find(queryBuilder);
        return await fluent.ToListAsync();
    }
}