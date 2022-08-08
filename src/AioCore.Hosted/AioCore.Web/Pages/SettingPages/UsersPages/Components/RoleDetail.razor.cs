using AioCore.Domain.IdentityAggregate;
using Microsoft.AspNetCore.Components;

namespace AioCore.Web.Pages.SettingPages.UsersPages.Components;

public partial class RoleDetail
{
    [Parameter] public RoleResponse Role { get; set; } = new();
    private Guid _selectedRole;
    private List<RoleResponse> _roles = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private void OnFinishFailed()
    {
    }

    private void OnSelectedRole(RoleResponse obj)
    {
    }
}