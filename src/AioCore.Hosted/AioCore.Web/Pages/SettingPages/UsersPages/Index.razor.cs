using AioCore.Domain.IdentityAggregate;
using AioCore.Read.IdentityQueries.RoleQueries;
using AioCore.Read.IdentityQueries.UserQueries;
using AioCore.Shared.Extensions;
using AioCore.Web.Services;
using AioCore.Write.IdentityCommands.RoleCommands;
using AioCore.Write.IdentityCommands.UserCommands;
using AntDesign;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace AioCore.Web.Pages.SettingPages.UsersPages;

public partial class Index
{
    private bool _modalUserDetailVisible;
    private bool _modalRoleDetailVisible;
    private string? _searchRoleKey;
    private Guid? _selectedRoleId;
    private RoleResponse? _role = new();
    private List<RoleResponse> _roles = new();
    private readonly UserResponse _user = new();
    private List<UserResponse> _users = new();

    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IAlertService Alert { get; set; } = default!;

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
        var response = await Mediator.Send(_role.To<CreateRoleCommand>());
        _modalRoleDetailVisible = false;
        await FetchRolesAsync();
        StateHasChanged();
        if (response.Success) await Alert.Success(response.Message);
        {
            await Alert.Error(response.Message);
        }
    }

    private void OnSelectRole(TreeEventArgs<RoleResponse> args)
    {
        _selectedRoleId = args.Node.Key.ToGuid();
        _role = _roles.First(x => x.Id.Equals(_selectedRoleId));
    }
}