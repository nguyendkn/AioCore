namespace AioCore.Shared.Common.Constants;

public static class SystemFeatures
{
    public const string Home = "/static/setting/index";
    public const string Tenants = "/static/settings/tenant/list";
    public const string Entities = "/static/settings/tenants/{0}/entity/list";
    public const string Forms = "/static/settings/tenants/{0}/form/list";
    public const string Views = "/static/settings/tenants/{0}/view/list";
    public const string Features = "/static/settings/feature/list";
    public const string Builder = "/static/settings/builder/{0}";

    public static List<string> Authorized = new()
    {
        "/_blazor",
        "/identity",
        "/static"
    };
}