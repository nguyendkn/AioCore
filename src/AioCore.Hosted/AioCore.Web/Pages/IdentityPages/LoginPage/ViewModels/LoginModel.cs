namespace AioCore.Web.Pages.IdentityPages.LoginPage.ViewModels;

public class LoginModel
{
    public string Email { get; set; } = default!;

    public string? Password { get; set; } = default!;

    public bool RememberMe { get; set; }
}