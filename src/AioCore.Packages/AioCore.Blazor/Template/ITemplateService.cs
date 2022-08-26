namespace AioCore.Blazor.Template;

public interface ITemplateService
{
    Task<string> RenderTemplateAsync<TViewModel>(string filename, TViewModel viewModel);
}