using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            if (request.Id.Equals(Guid.Empty))
            {
                if (string.IsNullOrEmpty(_appSettings.TenantConfigs.AssemblySavedFolder) || request.TenantId is null)
                    return new Response<SettingCode>
                    {
                        Message = Messages.UnspecifiedException,
                        Success = false
                    };
            
                var tenantFolder = Path.Combine(_appSettings.TenantConfigs.AssemblySavedFolder, request.TenantId.ToString() ?? string.Empty);
                if (!Directory.Exists(_appSettings.TenantConfigs.AssemblySavedFolder))
                    Directory.CreateDirectory(_appSettings.TenantConfigs.AssemblySavedFolder);
                if (!Directory.Exists(tenantFolder))
                    Directory.CreateDirectory(tenantFolder);
                var pathFile = Path.Combine(tenantFolder, request.Name);
                if (File.Exists(pathFile)) pathFile.DeleteFile();
                if (!string.IsNullOrEmpty(request.Code)) pathFile.WriteFile(request.Code);
                var entityEntry = await _context.Codes.AddAsync(request, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<SettingCode>
                {
                    Data = entityEntry.Entity,
                    Message = Messages.CreateDataSuccessful,
                    Success = true
                };
            }
            else
            {
                if (string.IsNullOrEmpty(_appSettings.TenantConfigs.AssemblySavedFolder) || request.TenantId is null)
                    return new Response<SettingCode>
                    {
                        Message = Messages.UnspecifiedException,
                        Success = false
                    };
                var code = await _context.Codes.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
                if (code is null) return new Response<SettingCode>
                {
                    Message = Messages.DataNotFound,
                    Success = false
                };
                code.Update(request.Name, request.Code);
                await _context.SaveChangesAsync(cancellationToken);
                var tenantFolder = Path.Combine(_appSettings.TenantConfigs.AssemblySavedFolder, request.TenantId.ToString() ?? string.Empty);
                var pathFile = Path.Combine(tenantFolder, request.Name);
                if (File.Exists(pathFile)) pathFile.DeleteFile();
                pathFile.WriteFile(request.Code);
                return new Response<SettingCode>
                {
                    Data = code,
                    Message = Messages.UpdateDataSuccessful,
                    Success = true
                };
            }
        }
    }

    public void Update(Guid? tenantId, Guid? parentId)
    {
        TenantId = tenantId.Equals(Guid.Empty) ? null : tenantId;
        ParentId = parentId.Equals(Guid.Empty) ? null : parentId;
    }
}