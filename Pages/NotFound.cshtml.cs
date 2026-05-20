using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarvelManager.Pages;

[AllowAnonymous]
public class NotFoundModel : PageModel
{
    public void OnGet()
    {
    }
}