// using AioCore.Domain.DatabaseContexts;
// using AioCore.Domain.DynamicAggregate;
// using AioCore.Domain.SettingAggregate;
// using AioCore.Services;
// using AioCore.Shared.Common.Constants;
// using AioCore.Shared.Extensions;
// using AioCore.Shared.SeedWorks;
// using AioCore.Shared.ValueObjects;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
//
// namespace AioCore.Write.DynamicCommands;
//
// public class UpdateDynamicEntityCommand : IRequest<Response<bool>>
// {
//     public string Name { get; set; } = default!;
//
//     public Guid EntityId { get; set; } = default!;
//
//     public Dictionary<string, object> Values { get; set; } = default!;
//
//     internal class Handler : IRequestHandler<UpdateDynamicEntityCommand, Response<bool>>
//     {
//         private readonly IClientService _clientService;
//         private readonly DynamicContext _dynamicContext;
//
//         public Handler(IClientService clientService, DynamicContext dynamicContext)
//         {
//             _clientService = clientService;
//             _dynamicContext = dynamicContext;
//         }
//
//         public async Task<Response<bool>> Handle(UpdateDynamicEntityCommand request,
//             CancellationToken cancellationToken)
//         {
//             var tenant = await _clientService.Tenant();
//             var dynamicEntity = await _dynamicContext
//                 .Entities
//                 .Include(t => t.DateValues)
//                 .Include(t => t.FloatValues)
//                 .Include(t => t.GuidValues)
//                 .Include(t => t.IntegerValues)
//                 .Include(t => t.StringValues)
//                 .FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);
//             if (dynamicEntity is null || dynamicEntity.TenantId != tenant.Id)
//                 throw new ApplicationException(Messages.EntityNotFound);
//
//             var attributes = await _dynamicContext.Attributes
//                 .Where(t => t.EntityTypeId == dynamicEntity.EntityTypeId)
//                 .ToListAsync(cancellationToken);
//
//             void Update<T, TType>(ICollection<T> dynamicValues, AttributeType dataType)
//                 where T : DynamicValue<TType>, new()
//             {
//                 var attrValues = attributes.Select(t =>
//                     {
//                         var @value = request.Values.FirstOrDefault(
//                             x => t.DataType == dataType && x.Key == t.Name);
//                         return new { t.Id, @value.Value };
//                     })
//                     .ToList();
//
//                 //update existing value
//                 foreach (var dynamicValue in dynamicValues)
//                 {
//                     var attr = attrValues.FirstOrDefault(t => t.Id == dynamicValue.AttributeId);
//                     if (attr is null) continue;
//                     if (!attr.Value.TryConvertTo<TType>(out var val)) continue;
//                     if (val != null) dynamicValue.Value = val;
//                 }
//
//                 //add new value
//                 foreach (var attr in attrValues.Where(t => dynamicValues.All(x => x.AttributeId != t.Id)))
//                 {
//                     if (!attr.Value.TryConvertTo<TType>(out var val)) continue;
//                     if (val != null)
//                         dynamicValues.Add(new T
//                         {
//                             EntityTypeId = dynamicEntity.EntityTypeId,
//                             AttributeId = attr.Id,
//                             Value = val
//                         });
//                 }
//             }
//
//             dynamicEntity.Name = request.Name;
//
//             Update<DynamicDateValue, DateTime>(dynamicEntity.DateValues, AttributeType.DateTime);
//             Update<DynamicIntegerValue, int>(dynamicEntity.IntegerValues, AttributeType.Number);
//             Update<DynamicFloatValue, float>(dynamicEntity.FloatValues, AttributeType.Float);
//             Update<DynamicGuidValue, Guid>(dynamicEntity.GuidValues, AttributeType.Guid);
//             Update<DynamicStringValue, string>(dynamicEntity.StringValues, AttributeType.Text);
//             await _dynamicContext.SaveChangesAsync(cancellationToken);
//
//             return new Response<bool>
//             {
//                 Data = true,
//                 Message = Messages.UpdateDataSuccessful,
//                 Success = true
//             };
//         }
//     }
// }