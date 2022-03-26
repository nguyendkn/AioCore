using Microsoft.Extensions.DependencyInjection;

namespace AioCore.Fluid.Core;

public static class FluidCoreExtension
{
    public static void AddFluidCore(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<FluidCoreParser>();
        services.AddSingleton<IFluidCoreService, FluidCoreService>();
    }
}