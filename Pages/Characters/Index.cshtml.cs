using MarvelManager.Models;
using MarvelManager.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarvelManager.Pages.Characters;

public class IndexModel : PageModel
{
    private readonly CharacterService _characterService;

    public List<MarvelCharacter> Characters { get; set; } = new();

    public IndexModel(CharacterService characterService)
    {
        _characterService = characterService;
    }

    public void OnGet()
    {
        Characters = _characterService.GetAll();
    }
}