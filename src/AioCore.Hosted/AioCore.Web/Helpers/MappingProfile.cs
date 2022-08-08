using AioCore.Domain.IdentityAggregate;
using AioCore.Write.IdentityCommands.RoleCommands;
using AutoMapper;

namespace AioCore.Web.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Role, RoleResponse>();
        CreateMap<CreateRoleCommand, Role>();
        CreateMap<User, UserResponse>();
    }
}