using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarvelManager.Pages.Account;

public class LogoutModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        TempData["SuccessMessage"] = "Vous avez été déconnecté avec succès.";

        await HttpContext.SignOutAsync("CookieAuth");

        return RedirectToPage("/Index");
    }
}