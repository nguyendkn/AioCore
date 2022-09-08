using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace AioCore.Shared.Hangfire;

public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}