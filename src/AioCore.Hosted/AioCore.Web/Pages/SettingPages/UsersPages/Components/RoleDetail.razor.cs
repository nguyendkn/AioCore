using AioCore.Domain.IdentityAggregate;
using Microsoft.AspNetCore.Components;

namespace AioCore.Web.Pages.SettingPages.UsersPages.Components;

public partial class RoleDetail
{
    [Parameter] public RoleResponse SelectedRole { get; set; } = new();
    [Parameter] public List<RoleResponse> Roles { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}