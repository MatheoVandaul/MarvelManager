using System.Text;
using MarvelManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarvelManager.Pages.Characters;

[Authorize(Roles = "Admin")]
public class ExportCsvModel : PageModel
{
    private readonly CharacterService _characterService;

    public ExportCsvModel(CharacterService characterService)
    {
        _characterService = characterService;
    }

    public FileResult OnGet()
    {
        var characters = _characterService.GetAllForExport();

        var builder = new StringBuilder();

        builder.AppendLine("HeroName,Name,Power,PowerLevel,Team");

        foreach (var character in characters)
        {
            builder.AppendLine(
                $"{character.HeroName}," +
                $"{character.Name}," +
                $"{character.Power}," +
                $"{character.PowerLevel}," +
                $"{character.TeamName}"
            );
        }

        return File(
            Encoding.UTF8.GetBytes(builder.ToString()),
            "text/csv",
            "characters.csv"
        );
    }
}