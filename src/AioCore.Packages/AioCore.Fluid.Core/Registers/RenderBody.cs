using Fluid;
using Fluid.Ast;
using Microsoft.Extensions.Caching.Memory;

namespace AioCore.Fluid.Core.Registers;

public static class RenderBody
{
    public static void Register(this FluidCoreParser parser, HttpClient httpClient, IMemoryCache memoryCache)
    {
        parser.RegisterEmptyTag(FluidConstants.RenderBody, static async (writer, encoder, context) =>
        {
            if (context.AmbientValues.TryGetValue(FluidConstants.BodyIndex, out var body))
            {
                await writer.WriteAsync((string) body);
            }
            else
            {
                throw new ParseException("Could not render body, Layouts can't be evaluated directly.");
            }

            return Completion.Normal;
        });
    }
}