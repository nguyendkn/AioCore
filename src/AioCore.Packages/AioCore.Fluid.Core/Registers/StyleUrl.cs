using System.Text;
using Fluid.Ast;
using Microsoft.Extensions.Caching.Memory;

namespace AioCore.Fluid.Core.Registers;

public static class StyleUrl
{
    public static void Register(this FluidCoreParser parser, HttpClient httpClient, IMemoryCache memoryCache)
    {
        parser.RegisterEmptyBlock(FluidConstants.StylesUrl, async (statements, textWriter, encoder, context) =>
        {
            var styleBuilder = new StringBuilder();
            styleBuilder.AppendLine("<style>");
            foreach (var styleUrl in from TextSpanStatement? textSpan in statements.ToList()
                     select textSpan?.Text.ToString().Trim())
            {
                var styleTemplate = memoryCache.Get<string>(styleUrl);
                if (string.IsNullOrEmpty(styleTemplate))
                {
                    styleTemplate = await httpClient.GetStringAsync(styleUrl);
                    memoryCache.Set(styleUrl, styleTemplate);
                }

                styleBuilder.AppendLine(styleTemplate.Trim());
            }

            styleBuilder.AppendLine("</style>");
            await textWriter.WriteAsync(styleBuilder);

            return Completion.Normal;
        });
    }
}