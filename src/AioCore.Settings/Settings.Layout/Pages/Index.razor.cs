using MediatR;
using Microsoft.AspNetCore.Components;
using Settings.Layout.Application.Commands;

namespace Settings.Layout.Pages;

public class IndexViewModel : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await Mediator.Send(new CreateLayoutCommand());
        await base.OnInitializedAsync();
    }
}