using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Write.SettingCommands.EntityCommands;

public class SubmitEntityCommand : SettingEntity, IRequest<Response<SettingEntity>>
{
    public SubmitEntityCommand(string name)
    {
        Name = name;
        CreatedAt = DateTime.Now;
        ModifiedAt = DateTime.Now;
    }

    public SubmitEntityCommand(Guid? id, Guid tenantId, string? name, DataSource dataSource, string? sourcePath)
    {
        Id = id ?? Guid.Empty;
        TenantId = tenantId;
        Name = string.IsNullOrEmpty(name) ? Name : name;
        DataSource = dataSource;
        SourcePath = string.IsNullOrEmpty(sourcePath) ? SourcePath : sourcePath;
    }

    internal class Handler : IRequestHandler<SubmitEntityCommand, Response<SettingEntity>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<SettingEntity>> Handle(SubmitEntityCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id.Equals(Guid.Empty))
            {
                var entityEntry = await _context.Entities.AddAsync(request, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<SettingEntity>
                {
                    Data = entityEntry.Entity,
                    Message = Messages.CreateDataSuccessful,
                    Success = true
                };
            }
            else
            {
                var entity = await _context.Entities.FirstOrDefaultAsync(
                    x => x.Id.Equals(request.Id), cancellationToken);
                if (entity is null) return new Response<SettingEntity>
                {
                    Message = Messages.DataNotFound,
                    Success = false
                };
                entity.Update(request.Name, request.DataSource, request.SourcePath);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<SettingEntity>
                {
                    Data = entity,
                    Message = Messages.UpdateDataSuccessful,
                    Success = true
                };
            }
        }
    }
}