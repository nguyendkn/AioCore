using AioCore.Domain.IdentityAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AioCore.Write.IdentityCommands.RoleCommands;

public class CreateRoleCommand : IRequest<Response<RoleResponse>>
{
    public string DraftName { get; set; } = default!;

    public string Name { get; set; } = default!;

    public Guid? ParentId { get; set; }

    internal class Handler : IRequestHandler<CreateRoleCommand, Response<RoleResponse>>
    {
        private readonly RoleManager<Role> _roleManager;

        public Handler(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Response<RoleResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            request.Name = request.DraftName;
            var role = await _roleManager.FindByNameAsync(request.Name);
            if (role is not null)
                return new Response<RoleResponse>
                {
                    Message = Messages.DataNotFound,
                    Success = false,
                };
            role = request.To<Role>();
            if (role.ParentId.Equals(Guid.Empty)) role.ParentId = null;
            var entityResult = await _roleManager.CreateAsync(role);
            if (entityResult.Succeeded)
                return new Response<RoleResponse>
                {
                    Data = request.To<RoleResponse>(),
                    Message = Messages.CreateDataSuccessful,
                    Success = true
                };
            return new Response<RoleResponse>
            {
                Message = Messages.CreateDataFailure,
                Success = false
            };
        }
    }
}