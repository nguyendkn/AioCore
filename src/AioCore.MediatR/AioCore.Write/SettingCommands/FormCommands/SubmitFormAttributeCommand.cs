using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Write.SettingCommands.FormCommands;

public class SubmitFormAttributeCommand : SettingFormAttribute, IRequest<Response<SettingFormAttribute>>
{
    public int? OrderStep { get; set; }
    
    internal class Handler : IRequestHandler<SubmitFormAttributeCommand, Response<SettingFormAttribute>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<SettingFormAttribute>> Handle(SubmitFormAttributeCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.Equals(Guid.Empty))
            {
                var entityEntry = await _context.FormAttributes.AddAsync(request, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<SettingFormAttribute>
                {
                    Data = entityEntry.Entity,
                    Message = Messages.CreateDataSuccessful,
                    Success = true
                };
            }

            if (request.OrderStep != null)
            {
                var attribute = await _context.Attributes.FirstOrDefaultAsync(
                    x => x.Id.Equals(request.Id), cancellationToken);
                if (attribute is not null)
                {
                    var nextAttribute = await _context.Attributes.Where(x => x.Order > attribute.Order)
                        .OrderBy(x => x.Order).FirstOrDefaultAsync(cancellationToken);
                    var previousAttribute = await _context.Attributes.Where(x => x.Order < attribute.Order)
                        .OrderByDescending(x => x.Order).FirstOrDefaultAsync(cancellationToken);
                    if (request.OrderStep.Equals(1) && nextAttribute is not null)
                    {
                        (nextAttribute.Order, attribute.Order) = (attribute.Order, nextAttribute.Order);
                        await _context.SaveChangesAsync(cancellationToken);
                        return new Response<SettingFormAttribute>
                        {
                            Message = Messages.UpdateDataSuccessful,
                            Success = true
                        };
                    }

                    if (request.OrderStep.Equals(-1) && previousAttribute is not null)
                    {
                        (previousAttribute.Order, attribute.Order) = (attribute.Order, previousAttribute.Order);
                        await _context.SaveChangesAsync(cancellationToken);
                        return new Response<SettingFormAttribute>
                        {
                            Message = Messages.UpdateDataSuccessful,
                            Success = true
                        };
                    }
                }
            }
            else
            {
                var attribute = await _context.FormAttributes.FirstOrDefaultAsync(
                    x => x.Id.Equals(request.Id), cancellationToken);
                if (attribute is null)
                    return new Response<SettingFormAttribute>
                    {
                        Message = Messages.DataNotFound,
                        Success = false
                    };
                attribute.Update(request.AttributeId, request.DisplayName);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<SettingFormAttribute>
                {
                    Data = attribute,
                    Message = Messages.UpdateDataSuccessful,
                    Success = true
                };
            }

            return new Response<SettingFormAttribute>
            {
                Message = Messages.UnspecifiedException,
                Success = false
            };
        }
    }
}