using AioCore.Domain.IdentityAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.IdentityQueries.UserQueries;

public class ListUserQuery : IRequest<Response<List<UserResponse>>>
{
    internal class Handler : IRequestHandler<ListUserQuery, Response<List<UserResponse>>>
    {
        private readonly UserManager<User> _userManager;

        public Handler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<List<UserResponse>>> Handle(ListUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync(cancellationToken);
            if (!users.Any()) return new Response<List<UserResponse>>
            {
                Message = Messages.DataNotFound,
                Success = false
            };
            return new Response<List<UserResponse>>
            {
                Data = users.To<List<UserResponse>>(),
                Success = true
            };
        }
    }
}