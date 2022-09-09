using System.ComponentModel.DataAnnotations;
using AioCore.Domain.IdentityAggregate;
using AioCore.Shared.Common.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AioCore.Web.Areas.Identity.Pages.Account;

public class LoginViewModel
{
    [Required] [EmailAddress] public string Email { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Ghi nhớ?")] public bool RememberMe { get; set; }
}

public class LoginModel : PageModel
{
    [BindProperty] public LoginViewModel Input { get; set; } = default!;

    public string? ReturnUrl { get; set; }

    [TempData] public string? ErrorMessage { get; set; }

    private readonly SignInManager<User> _signInManager;

    public LoginModel(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid) return Page();
        var signInResult = await _signInManager.PasswordSignInAsync(
            Input.Email.Split("@").First(), Input.Password,
            Input.RememberMe, lockoutOnFailure: false);

        if (signInResult.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "Tài khoản bị khóa");
            return RedirectToPage(SystemFeatures.Home);
        }

        if (signInResult.Succeeded)
        {
            return LocalRedirect(SystemFeatures.Home);
        }

        ModelState.AddModelError(string.Empty, "Đăng nhập không hợp lệ");
        return Page();
    }
}