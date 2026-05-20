using System.ComponentModel.DataAnnotations;
using MarvelManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarvelManager.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserService _userService;

    public RegisterModel(UserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Le nom est obligatoire")]
    public string Name { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Email invalide")]
    public string Email { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
    public string Password { get; set; } = "";

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        var existingUser = _userService.GetByEmail(Email);

        if (existingUser != null)
        {
            ErrorMessage = "Un compte existe déjà avec cet email.";
            return Page();
        }

        _userService.Register(Name, Email, Password);

        return RedirectToPage("/Account/Login");
    }
}