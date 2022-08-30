// using AioCore.Domain.DatabaseContexts;
// using AioCore.Services;
// using AioCore.Shared.Common.Constants;
// using AioCore.Shared.SeedWorks;
// using MediatR;
//
// namespace AioCore.Write.DynamicCommands;
//
// public class RemoveDynamicEntityCommand : IRequest<Response<bool>>
// {
//     public Guid Id { get; set; }
//
//     internal class Handler : IRequestHandler<RemoveDynamicEntityCommand, Response<bool>>
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
//         public async Task<Response<bool>> Handle(RemoveDynamicEntityCommand request,
//             CancellationToken cancellationToken)
//         {
//             var tenant = await _clientService.Tenant();
//             var dynamicEntity = await _dynamicContext
//                 .Entities.FindAsync(new object[] { request.Id }, cancellationToken);
//             if (dynamicEntity is null)
//             {
//                 throw new ApplicationException(Messages.EntityNotFound);
//             }
//
//             if (dynamicEntity.TenantId != tenant.Id)
//             {
//                 throw new ApplicationException(Messages.EntityNotFound);
//             }
//
//             _dynamicContext.Entities.Remove(dynamicEntity);
//
//             await _dynamicContext.SaveChangesAsync(cancellationToken);
//
//             return new Response<bool>
//             {
//                 Data = true,
//                 Message = Messages.RemoveDataSuccessful,
//                 Success = true
//             };
//         }
//     }
// }