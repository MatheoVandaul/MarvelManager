using MarvelManager.Models;
using MarvelManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace MarvelManager.Pages.Characters;


[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly CharacterService _characterService;
    private readonly TeamService _teamService;

    [BindProperty]
    public MarvelCharacter Character { get; set; } = new();

    public List<SelectListItem> TeamOptions { get; set; } = new();

    public CreateModel(
        CharacterService characterService,
        TeamService teamService)
    {
        _characterService = characterService;
        _teamService = teamService;
    }

    public void OnGet()
    {
        LoadTeams();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadTeams();
            return Page();
        }

        _characterService.Create(Character);

        return RedirectToPage("Index");
    }

    private void LoadTeams()
    {
        TeamOptions = _teamService.GetAll()
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            })
            .ToList();
    }
}