using AioCore.Domain.IdentityAggregate;
using AioCore.Read.IdentityQueries.RoleQueries;
using AioCore.Read.IdentityQueries.UserQueries;
using AioCore.Shared.Extensions;
using AioCore.Write.IdentityCommands.RoleCommands;
using AioCore.Write.IdentityCommands.UserCommands;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace AioCore.Web.Pages.SettingPages.UsersPages;

public partial class Index
{
    private bool _modalUserDetailVisible;
    private bool _modalRoleDetailVisible;
    private string? _searchRoleKey;
    private readonly RoleResponse _role = new();
    private List<RoleResponse> _roles = new();
    private readonly UserResponse _user = new();
    private List<UserResponse> _users = new();

    [Inject] private IMediator Mediator { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await FetchAsync();
        await base.OnInitializedAsync();
    }

    private async Task FetchAsync()
    {
        await FetchRolesAsync();
        await FetchUsersAsync();
    }

    private async Task FetchRolesAsync()
    {
        var response = await Mediator.Send(new ListRoleQuery());
        _roles = response.Data ?? default!;
    }

    private async Task FetchUsersAsync()
    {
        var response = await Mediator.Send(new ListUserQuery());
        _users = response.Data ?? default!;
    }

    private void OnToggleModalUserDetail()
    {
        _modalUserDetailVisible = !_modalUserDetailVisible;
    }
    
    private void OnToggleModalRoleDetail()
    {
        _modalRoleDetailVisible = !_modalRoleDetailVisible;
    }

    private async void OnSubmitUserDetail()
    {
        await Mediator.Send(_user.To<CreateUserCommand>());
    }

    private async void OnSubmitRoleDetail()
    {
        await Mediator.Send(_role.To<CreateRoleCommand>());
    }
}