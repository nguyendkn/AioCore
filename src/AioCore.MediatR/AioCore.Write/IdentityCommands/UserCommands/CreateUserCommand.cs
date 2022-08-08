using AioCore.Domain.IdentityAggregate;
using AioCore.Shared.SeedWorks;
using MediatR;

namespace AioCore.Write.IdentityCommands.UserCommands;

public class CreateUserCommand : IRequest<Response<UserResponse>>
{
    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public List<Guid> RoleIds { get; set; } = default!;
    
    internal class Handler : IRequestHandler<CreateUserCommand, Response<UserResponse>>
    {
        public Task<Response<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}