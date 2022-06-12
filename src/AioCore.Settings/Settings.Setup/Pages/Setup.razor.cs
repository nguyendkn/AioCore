using MediatR;
using Settings.Setup.Application.Commands;

namespace Settings.Setup.Pages;

public partial class Setup
{
    protected override async Task OnInitializedAsync()
    {
        await _mediatR.Send(new StartSetupCommand());
        await base.OnInitializedAsync();
    }
}