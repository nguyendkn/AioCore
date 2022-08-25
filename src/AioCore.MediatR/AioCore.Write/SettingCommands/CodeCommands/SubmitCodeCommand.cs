using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
using MediatR;

namespace AioCore.Write.SettingCommands.CodeCommands;

public class SubmitCodeCommand : SettingCode, IRequest<Response<SettingCode>>
{
    internal class Handler : IRequestHandler<SubmitCodeCommand, Response<SettingCode>>
    {
        private readonly SettingsContext _context;
        private readonly AppSettings _appSettings;

        public Handler(SettingsContext context, AppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public async Task<Response<SettingCode>> Handle(SubmitCodeCommand request, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(_appSettings.TenantConfigs.SavedFolder))
                Directory.CreateDirectory(_appSettings.TenantConfigs.SavedFolder);
            request.PathFile = Path.Combine(_appSettings.TenantConfigs.SavedFolder);
            var entityEntry = await _context.Codes.AddAsync(request, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response<SettingCode>
            {
                Data = entityEntry.Entity,
                Message = Messages.CreateDataSuccessful,
                Success = true
            };
        }
    }

    public void Update(Guid? parentId)
    {
        ParentId = parentId.Equals(Guid.Empty) ? null : parentId;
    }
}