using Fluid.Ast;
using Microsoft.Extensions.Caching.Memory;

namespace AioCore.Fluid.Core.Registers;

public static class Layout
{
    public static void Register(this FluidCoreParser parser, HttpClient httpClient, IMemoryCache memoryCache)
    {
        parser.RegisterExpressionTag(FluidConstants.Layout, static async (pathExpression, writer, encoder, context) =>
        {
            var layoutPath = (await pathExpression.EvaluateAsync(context)).ToStringValue();

            if (string.IsNullOrEmpty(layoutPath))
            {
                context.AmbientValues[FluidConstants.LayoutIndex] = null;
                return Completion.Normal;
            }

            context.AmbientValues[FluidConstants.LayoutIndex] = layoutPath;

            return Completion.Normal;
        });
    }
}