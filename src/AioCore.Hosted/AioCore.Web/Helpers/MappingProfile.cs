using AioCore.Domain.IdentityAggregate;
using AioCore.Web.Pages.SettingPages.TenantPages.ViewModels;
using AioCore.Write.IdentityCommands.RoleCommands;
using AioCore.Write.IdentityCommands.UserCommands;
using AioCore.Write.SettingCommands.TenantCommands;
using AutoMapper;

namespace AioCore.Web.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TenantDetailModel, SubmitTenantCommand>();
        
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