using MarvelManager.Models;
using MarvelManager.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarvelManager.Pages.Characters;


public class IndexModel : PageModel
{
    private readonly CharacterService _characterService;

    public List<MarvelCharacter> Characters { get; set; } = new();

    public string? Search { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public IndexModel(CharacterService characterService)
    {
        _characterService = characterService;
        
    }

    public void OnGet(string? search, int pageNumber = 1)
    {
        Search = search;
        CurrentPage = pageNumber;

        const int pageSize = 6;

        Characters = _characterService.GetAll(search, pageNumber, pageSize);

        var totalCharacters = _characterService.Count(search);
        TotalPages = (int)Math.Ceiling(totalCharacters / (double)pageSize);
        
    }
    
}
