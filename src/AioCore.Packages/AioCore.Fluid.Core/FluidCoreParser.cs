using System.Reflection;
using AioCore.Fluid.Core.Abstracts;
using Fluid;
using Microsoft.Extensions.Caching.Memory;

namespace AioCore.Fluid.Core;

public class FluidCoreParser : FluidParser, IFluidRegister
{
    public FluidCoreParser() : this(new FluidParserOptions())
    {
    }

    public FluidCoreParser(FluidParserOptions parserOptions) : base(parserOptions)
    {
        var httpClient = new HttpClient();
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var parameters = new object[] {this, httpClient, memoryCache};

        var types = typeof(Assembly).Assembly
            .GetExportedTypes().Where(x => x.IsAbstract);
        foreach (var type in types)
            type.GetMethod(nameof(Register), BindingFlags.Public | BindingFlags.Static)
                ?.Invoke(null, parameters);
    }

    public static void Register()
    {
    }
}