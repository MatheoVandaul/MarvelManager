using MarvelManager.Models;
using MarvelManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarvelManager.Pages.Characters;

public class DetailsModel : PageModel
{
    private readonly CharacterService _characterService;

    public MarvelCharacter Character { get; set; } = new();

    public DetailsModel(CharacterService characterService)
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
}