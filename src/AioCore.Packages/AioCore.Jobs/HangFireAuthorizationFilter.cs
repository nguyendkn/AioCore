using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace AioCore.Jobs;

public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}