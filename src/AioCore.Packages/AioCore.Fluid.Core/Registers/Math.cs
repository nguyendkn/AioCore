using Fluid.Ast;
using Microsoft.Extensions.Caching.Memory;
using Scriban;

namespace AioCore.Fluid.Core.Registers;

public static class Math
{
    public static void Register(this FluidCoreParser parser, HttpClient httpClient, IMemoryCache memoryCache)
    {
        parser.RegisterEmptyBlock(FluidConstants.Math,
            static async (statements, textWriter, encoder, context) =>
            {
                var statement = statements.ToList().FirstOrDefault() as TextSpanStatement;
                var mathTemplate = statement?.Text.ToString().Trim();
                var scribanTemplate = "{{" + mathTemplate + "}}";
                var parser = await Template.Parse(scribanTemplate).RenderAsync();
                await textWriter.WriteAsync(parser);

                return Completion.Normal;
            });
    }
}