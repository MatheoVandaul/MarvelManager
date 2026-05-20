using MarvelManager.Models;
using MarvelManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace MarvelManager.Pages.Characters;

[Authorize(Roles = "Admin")]
public class DeleteModel : PageModel
{
    private readonly CharacterService _characterService;

    public MarvelCharacter Character { get; set; } = new();

    public DeleteModel(CharacterService characterService)
    {
        _characterService = characterService;
    }

    public IActionResult OnGet(int id)
    {
        var character = _characterService.GetById(id);

        if (character == null)
            return NotFound();

        Character = character;
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        _characterService.Delete(id);
        return RedirectToPage("Index");
    }
}