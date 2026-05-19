using System.ComponentModel.DataAnnotations;

namespace MarvelManager.Models;

public class MarvelCharacter
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom réel est obligatoire")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Le nom de héros est obligatoire")]
    [StringLength(100, MinimumLength = 2)]
    public string HeroName { get; set; } = "";

    [Required(ErrorMessage = "Le pouvoir est obligatoire")]
    [StringLength(200, MinimumLength = 3)]
    public string Power { get; set; } = "";

    [Required]
    [Range(1, 100, ErrorMessage = "Le niveau doit être entre 1 et 100")]
    public int PowerLevel { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public int TeamId { get; set; }

    public string? TeamName { get; set; }
}