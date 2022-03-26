namespace AioCore.Fluid.Core;

public static class FluidConstants
{
    public const string SectionsIndex = "$$sections";
    public const string BodyIndex = "$$body";
    public const string LayoutIndex = "$$layout";

    public static readonly string RenderBody = nameof(RenderBody).ToLower();
    public static readonly string Section = nameof(Section).ToLower();
    public static readonly string Layout = nameof(Layout).ToLower();
    public static readonly string ComponentUrl = nameof(ComponentUrl).ToLower();
    public static readonly string StylesUrl = nameof(StylesUrl).ToLower();
    public static readonly string Math = nameof(Math).ToLower();
}