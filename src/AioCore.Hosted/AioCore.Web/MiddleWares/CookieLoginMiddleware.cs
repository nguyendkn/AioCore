using System.Collections.Concurrent;
using AioCore.Domain.IdentityAggregate;
using AioCore.Web.Pages.IdentityPages.LoginPage.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AioCore.Web.MiddleWares;

public class CookieLoginMiddleware
{
    public static IDictionary<Guid, LoginModel> Logins { get; private set; }
        = new ConcurrentDictionary<Guid, LoginModel>();


    private readonly RequestDelegate _next;

    public CookieLoginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, SignInManager<User> signInMgr)
    {
        if (context.Request.Path == "/static/identity/login" && context.Request.Query.ContainsKey("key"))
        {
            var key = Guid.Parse(context.Request.Query["key"]);
            var info = Logins[key];

            var result = await signInMgr.PasswordSignInAsync(info.Email, info.Password, false, lockoutOnFailure: true);
            info.Password = null;
            if (result.Succeeded)
            {
                Logins.Remove(key);
                context.Response.Redirect("/");
                return;
            }

            //TODO: Proper error handling
            context.Response.Redirect("/loginfailed");
            return;
        }

        await _next.Invoke(context);
    }
}