using MarvelManager.Models;
using MarvelManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace MarvelManager.Pages.Characters;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly CharacterService _characterService;
    private readonly TeamService _teamService;

    [BindProperty]
    public MarvelCharacter Character { get; set; } = new();

    public List<SelectListItem> TeamOptions { get; set; } = new();

    public EditModel(
        CharacterService characterService,
        TeamService teamService)
    {
        _characterService = characterService;
        _teamService = teamService;
    }

    public IActionResult OnGet(int id)
    {
        var character = _characterService.GetById(id);

        if (character == null)
            return NotFound();

        Character = character;

        LoadTeams();

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadTeams();
            return Page();
        }

        _characterService.Update(Character);

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