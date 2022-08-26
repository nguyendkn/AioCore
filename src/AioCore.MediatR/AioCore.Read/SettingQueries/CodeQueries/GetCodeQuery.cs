using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.CodeQueries;

public class GetCodeQuery : IRequest<Response<SettingCode>>
{
    public Guid? Id { get; set; }

    public GetCodeQuery(Guid? id)
    {
        Id = id;
    }
    
    internal class Handler : IRequestHandler<GetCodeQuery, Response<SettingCode>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<SettingCode>> Handle(GetCodeQuery request, CancellationToken cancellationToken)
        {
            var code = await _context.Codes.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            if (code is null) return new Response<SettingCode>
            {
                Message = Messages.DataNotFound,
                Success = false
            };
            return new Response<SettingCode>
            {
                Data = code,
                Success = true
            };
        }
    }
}