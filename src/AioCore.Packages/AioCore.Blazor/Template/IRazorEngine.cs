namespace AioCore.Blazor.Template;

public interface IRazorEngine
{
    IRazorEngineCompiledTemplate Compile<T>(string content, Action<IRazorEngineCompilationOptionsBuilder>? builderAction = null) 
        where T : IRazorEngineTemplate;
        
    Task<IRazorEngineCompiledTemplate> CompileAsync<T>(string content, Action<IRazorEngineCompilationOptionsBuilder>? builderAction = null) 
        where T : IRazorEngineTemplate;
        
    IRazorEngineCompiledTemplate Compile(string content, Action<IRazorEngineCompilationOptionsBuilder>? builderAction = null);
        
    Task<IRazorEngineCompiledTemplate> CompileAsync(string content, Action<IRazorEngineCompilationOptionsBuilder>? builderAction = null);
}