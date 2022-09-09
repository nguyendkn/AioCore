using System.Security.Claims;
using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.IdentityAggregate;
using AioCore.Shared.Extensions;
using AioCore.Shared.ValueObjects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Web.Providers;

public class ClaimsTransformation : IClaimsTransformation
{
    private readonly UserManager<User> _userManager;
    private readonly SettingsContext _settingsContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimsTransformation(
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor,
        SettingsContext settingsContext)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _settingsContext = settingsContext;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identities.FirstOrDefault(c => c.IsAuthenticated);
        if (identity == null) return principal;

        var user = await _userManager.GetUserAsync(principal);
        var host = _httpContextAccessor.HttpContext?.Request.Headers[RequestHeaders.Host].ToString();
        var tenantId = await _settingsContext.Tenants.FirstOrDefaultAsync(x => x.Domain.Equals(host));
        if (user == null) return principal;
        var roles = await _userManager.GetRolesAsync(user);
        identity.AddClaim(new Claim(nameof(UserClaimsValue.Id), user.Id.ToString()));
        identity.AddClaim(new Claim(nameof(UserClaimsValue.UserName), user.Id.ToString()));
        identity.AddClaim(new Claim(nameof(UserClaimsValue.Email), user.Email));
        identity.AddClaim(new Claim(nameof(UserClaimsValue.Host), host ?? string.Empty));
        identity.AddClaim(new Claim(nameof(UserClaimsValue.TenantId), tenantId?.Id.ToString() ?? string.Empty));
        identity.AddClaim(new Claim(nameof(UserClaimsValue.Roles), roles.Serialize()));

        if (!principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
        {
            identity.AddClaim(new Claim(nameof(UserClaimsValue.FullName), user.FullName));
        }

        return new ClaimsPrincipal(identity);
    }
}