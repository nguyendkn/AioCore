using Fluid;
using Fluid.Ast;

namespace AioCore.Fluid.Core;

public class FluidCoreParser : FluidParser
{
    public FluidCoreParser() : this(new FluidParserOptions())
    {
    }

    public FluidCoreParser(FluidParserOptions parserOptions) : base(parserOptions)
    {
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

        RegisterExpressionTag(FluidConstants.TemplateUrl,
            static async (pathExpression, writer, encoder, context) =>
            {
                var tmp = 1 + 1;
                return Completion.Normal;
            });
    }
}