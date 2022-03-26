using Fluid.Ast;
using System.Text;
using Fluid;
using Microsoft.Extensions.Caching.Memory;
using Scriban;

namespace AioCore.Fluid.Core;

public class FluidCoreParser : FluidParser
{
    public FluidCoreParser() : this(new FluidParserOptions())
    {
    }

    public FluidCoreParser(FluidParserOptions parserOptions) : base(parserOptions)
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var httpClient = new HttpClient();

        #region ComponentUrl

        RegisterEmptyBlock(FluidConstants.ComponentUrl, async (statements, textWriter, encoder, context) =>
        {
            var componentUrl = (statements.ToList().FirstOrDefault() as TextSpanStatement)?.Text.ToString().Trim();
            var componentTemplate =
                memoryCache.Get<string>(componentUrl) ?? await httpClient.GetStringAsync(componentUrl);
            await textWriter.WriteAsync(componentTemplate);
            return Completion.Normal;
        });

        #endregion

        #region Layout

        RegisterExpressionTag(FluidConstants.Layout, static async (pathExpression, writer, encoder, context) =>
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

        #endregion

        #region Math

        RegisterEmptyBlock(FluidConstants.Math,
            static async (statements, textWriter, encoder, context) =>
            {
                var statement = statements.ToList().FirstOrDefault() as TextSpanStatement;
                var mathTemplate = statement?.Text.ToString().Trim();
                var scribanTemplate = "{{" + mathTemplate + "}}";
                var parser = await Template.Parse(scribanTemplate).RenderAsync();
                await textWriter.WriteAsync(parser);

                return Completion.Normal;
            });

        #endregion

        #region RenderBody

        RegisterEmptyTag(FluidConstants.RenderBody, static async (writer, encoder, context) =>
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

        #endregion

        #region Section

        RegisterIdentifierBlock(FluidConstants.Section, static (identifier, statements, writer, encoder, context) =>
        {
            if (!context.AmbientValues.TryGetValue(FluidConstants.SectionsIndex, out var sections))
                return new ValueTask<Completion>(Completion.Normal);
            if (sections is not Dictionary<string, IReadOnlyList<Statement>> dictionary)
            {
                dictionary = new Dictionary<string, IReadOnlyList<Statement>>();
                context.AmbientValues[FluidConstants.SectionsIndex] = dictionary;
            }

            dictionary[identifier] = statements;

            return new ValueTask<Completion>(Completion.Normal);
        });

        #endregion

        #region StyleUrl

        RegisterEmptyBlock(FluidConstants.StylesUrl, async (statements, textWriter, encoder, context) =>
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

        #endregion
    }
}