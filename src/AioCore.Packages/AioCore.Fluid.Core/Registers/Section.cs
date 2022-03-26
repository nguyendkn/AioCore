using Fluid.Ast;
using Microsoft.Extensions.Caching.Memory;

namespace AioCore.Fluid.Core.Registers;

public static class Section
{
    public static void Register(FluidCoreParser parser, HttpClient httpClient, IMemoryCache memoryCache)
    {
        parser.RegisterIdentifierBlock(FluidConstants.Section,
            static (identifier, statements, writer, encoder, context) =>
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
    }
}