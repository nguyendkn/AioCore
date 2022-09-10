using AioCore.Shared.ValueObjects;
using Microsoft.AspNetCore.Components.Authorization;

namespace AioCore.Shared.Extensions;

public static class StateProviderExtension
{
    public static UserClaimsValue UserClaims(this AuthenticationStateProvider stateProvider)
    {
        var authenticationState = stateProvider.GetAuthenticationStateAsync().Result;
        var claims = authenticationState.User.Claims.ToList();
        return new UserClaimsValue
        {
            Id = claims.FirstOrDefault(x => x.Type.Equals(nameof(UserClaimsValue.Id)))?.Value.ToGuid() ?? Guid.Empty,
            FullName = claims.FirstOrDefault(x => x.Type.Equals(nameof(UserClaimsValue.FullName)))?.Value,
            UserName = claims.FirstOrDefault(x => x.Type.Equals(nameof(UserClaimsValue.UserName)))?.Value,
            Email = claims.FirstOrDefault(x => x.Type.Equals(nameof(UserClaimsValue.Email)))?.Value,
            Roles = claims.FirstOrDefault(x => x.Type.Equals(nameof(UserClaimsValue.Roles)))?.Value
                .Deserialize<List<string>>(),
            TenantId = claims.FirstOrDefault(x => x.Type.Equals(nameof(UserClaimsValue.TenantId)))?.Value.ToGuid() ??
                       Guid.Empty,
        };
    }
}