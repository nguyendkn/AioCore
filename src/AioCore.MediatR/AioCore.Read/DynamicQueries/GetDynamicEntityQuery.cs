// using AioCore.Domain.DatabaseContexts;
// using AioCore.Shared.Common.Constants;
// using AioCore.Shared.SeedWorks;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
//
// namespace AioCore.Read.DynamicQueries;
//
// public class GetDynamicEntityQuery : IRequest<Response<string>>
// {
//     public Guid Id { get; set; }
//
//     public GetDynamicEntityQuery(Guid id)
//     {
//         Id = id;
//     }
//
//     internal class Handler : IRequestHandler<GetDynamicEntityQuery, Response<string>>
//     {
//         private readonly DynamicContext _dynamicContext;
//
//         public Handler(DynamicContext dynamicContext)
//         {
//             _dynamicContext = dynamicContext;
//         }
//
//         public async Task<Response<string>> Handle(GetDynamicEntityQuery request,
//             CancellationToken cancellationToken)
//         {
//             var entity = await _dynamicContext.Entities.FirstOrDefaultAsync(
//                 x => x.Id.Equals(request.Id), cancellationToken);
//             if (entity is null) return new Response<string>
//             {
//                 Message = Messages.DataNotFound,
//                 Success = false
//             };
//             return new Response<string>
//             {
//                 Data = entity.Data,
//                 Success = true
//             };
//         }
//     }
// }