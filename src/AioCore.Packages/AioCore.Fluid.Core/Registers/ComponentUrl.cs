using Fluid.Ast;
using Microsoft.Extensions.Caching.Memory;

namespace AioCore.Fluid.Core.Registers;

public static class ComponentUrl
{
    public static void Register(this FluidCoreParser parser, HttpClient httpClient, IMemoryCache memoryCache)
    {
        parser.RegisterEmptyBlock(FluidConstants.ComponentUrl, async (statements, textWriter, encoder, context) =>
        {
            var componentUrl = (statements.ToList().FirstOrDefault() as TextSpanStatement)?.Text.ToString().Trim();
            var componentTemplate =
                memoryCache.Get<string>(componentUrl) ?? await httpClient.GetStringAsync(componentUrl);
            await textWriter.WriteAsync(componentTemplate);
            return Completion.Normal;
        });
    }
}