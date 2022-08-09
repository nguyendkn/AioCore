using AioCore.Domain.IdentityAggregate;
using AioCore.Write.IdentityCommands.RoleCommands;
using AioCore.Write.IdentityCommands.UserCommands;
using AutoMapper;

namespace AioCore.Web.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Role, RoleResponse>();
        CreateMap<RoleResponse, Role>();
        CreateMap<CreateRoleCommand, Role>();
        CreateMap<Role, CreateRoleCommand>();
        CreateMap<RoleResponse, CreateRoleCommand>();
        CreateMap<CreateRoleCommand, RoleResponse>();
        
        CreateMap<User, UserResponse>();
        CreateMap<UserResponse, User>();
        CreateMap<CreateUserCommand, User>();
        CreateMap<User, CreateUserCommand>();
        CreateMap<UserResponse, CreateUserCommand>();
        CreateMap<CreateUserCommand, UserResponse>();
    }
}