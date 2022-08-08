using AioCore.Domain.IdentityAggregate;
using AioCore.Read.IdentityQueries.RoleQueries;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace AioCore.Web.Pages.SettingPages.UsersPages;

public partial class Index
{
    private bool _modalUserDetailVisible;
    private bool _modalRoleDetailVisible;
    private string _searchRoleKey;
    private List<RoleResponse> _roles = new();

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
    
    private async Task FetchUsersAsync() {}

    private void OnToggleModalUserDetail()
    {
        _modalUserDetailVisible = !_modalUserDetailVisible;
    }
    
    private void OnToggleModalRoleDetail()
    {
        _modalRoleDetailVisible = !_modalRoleDetailVisible;
    }

    private void OnSubmitUserDetail()
    {
    }
    
    
    private void OnSubmitRoleDetail()
    {
    }
}