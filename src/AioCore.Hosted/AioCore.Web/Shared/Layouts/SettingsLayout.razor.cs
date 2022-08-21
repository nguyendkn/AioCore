using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AioCore.Web.Shared.Layouts;

public partial class SettingsLayout
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }
}