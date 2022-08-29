using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.DynamicAggregate;
using AioCore.Domain.SettingAggregate;
using AioCore.Services;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Write.DynamicCommands;

public class CreateDynamicEntityCommand : IRequest<Response<bool>>
{
    public string Name { get; set; } = default!;

    public Guid EntityTypeId { get; set; } = default!;

    public Dictionary<string, object> Value { get; set; } = default!;

    internal class Handler : IRequestHandler<CreateDynamicEntityCommand, Response<bool>>
    {
        private readonly IClientService _clientService;
        private readonly SettingsContext _settingsContext;
        private readonly DynamicContext _dynamicContext;

        public Handler(SettingsContext settingsContext, IClientService clientService, DynamicContext dynamicContext)
        {
            _settingsContext = settingsContext;
            _clientService = clientService;
            _dynamicContext = dynamicContext;
        }

        public async Task<Response<bool>> Handle(CreateDynamicEntityCommand request,
            CancellationToken cancellationToken)
        {
            var currentTenant = await _clientService.Tenant();
            if (currentTenant == null)
            {
                throw new ApplicationException("Current tenant does not exists");
            }

            var entityType =
                await _settingsContext.Entities.FindAsync(new object[] { request.EntityTypeId },
                    cancellationToken: cancellationToken);
            if (entityType == null)
            {
                throw new ApplicationException("Current type does not exists");
            }

            if (entityType.Id != currentTenant.Id)
            {
                throw new ApplicationException("Current type does not exists for this tenant");
            }

            var entity = await _dynamicContext.Entities.AddAsync(new DynamicEntity
            {
                Name = request.Name,
                EntityTypeId = request.EntityTypeId,
                TenantId = currentTenant.Id,
            }, cancellationToken);

            var attributes = await _dynamicContext.Attributes.Where(t => t.EntityTypeId == entityType.Id)
                .ToListAsync(cancellationToken);

            List<T?> CreateDynamicValues<T, TType>(AttributeType dataType) where T : DynamicValue<TType>, new()
            {
                return attributes.Where(t => t.DataType == dataType)
                    .Select(t =>
                    {
                        var value = request.Value.FirstOrDefault(x => x.Key == t.Name);
                        return CreateValue<T, TType>(entity.Entity.Id, entity.Entity.EntityTypeId, t.Id, value.Value);
                    })
                    .Where(t => t != null)
                    .ToList();
            }

            var dynamicDateValues = CreateDynamicValues<DynamicDateValue, DateTime>(AttributeType.DateTime);
            var dynamicIntValues = CreateDynamicValues<DynamicIntegerValue, int>(AttributeType.Number);
            var dynamicFloatValues = CreateDynamicValues<DynamicFloatValue, float>(AttributeType.Float);
            var dynamicGuidValues = CreateDynamicValues<DynamicGuidValue, Guid>(AttributeType.Guid);
            var dynamicStringValues = CreateDynamicValues<DynamicStringValue, string>(AttributeType.Text);

            if (dynamicDateValues.Any())
                await _dynamicContext.DateValues.AddRangeAsync(dynamicDateValues!, cancellationToken);

            if (dynamicFloatValues.Any())
                await _dynamicContext.FloatValues.AddRangeAsync(dynamicFloatValues!, cancellationToken);

            if (dynamicGuidValues.Any())
                await _dynamicContext.GuidValues.AddRangeAsync(dynamicGuidValues!, cancellationToken);

            if (dynamicIntValues.Any())
                await _dynamicContext.IntegerValues.AddRangeAsync(dynamicIntValues!, cancellationToken);

            if (dynamicStringValues.Any())
                await _dynamicContext.StringValues.AddRangeAsync(dynamicStringValues!, cancellationToken);

            await _dynamicContext.SaveChangesAsync(cancellationToken);

            return new Response<bool>
            {
                Data = true,
                Message = Messages.CreateDataSuccessful,
                Success = true
            };
        }

        private static T? CreateValue<T, TType>(Guid entityId, Guid typeId, Guid attributeId, object value)
            where T : DynamicValue<TType>, new()
        {
            var instance = new T
            {
                EntityId = entityId,
                EntityTypeId = typeId,
                AttributeId = attributeId
            };

            if (!value.TryConvertTo<TType>(out var convertedValue)) return null;
            if (convertedValue != null) instance.Value = convertedValue;
            return instance;
        }
    }
}