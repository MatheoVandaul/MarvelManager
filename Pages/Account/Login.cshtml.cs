using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MarvelManager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarvelManager.Pages.Account;

public class LoginModel : PageModel
{
    private readonly UserService _userService;

    public LoginModel(UserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Email invalide")]
    public string Email { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    public string Password { get; set; } = "";

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var isValid = _userService.ValidateLogin(Email, Password, out var user);

        if (!isValid || user == null)
        {
            ErrorMessage = "Email ou mot de passe incorrect.";
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("CookieAuth", principal);
        TempData["SuccessMessage"] = $"Bienvenue {user.Name}, vous êtes connecté.";

        return RedirectToPage("/Index");
    }
}