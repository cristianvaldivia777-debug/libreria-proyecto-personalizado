using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models;

public class Libro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio"), StringLength(150)]
    public string Titulo { get; set; } = "";

    [Required(ErrorMessage = "El autor es obligatorio"), StringLength(100)]
    public string Autor { get; set; } = "";

    [StringLength(20)]
    public string? ISBN { get; set; }

    [Range(1000, 2100, ErrorMessage = "Año inválido")]
    [Display(Name = "Año")]
    public int Anio { get; set; } = DateTime.Now.Year;

    [Range(0, 1000)]
    [Display(Name = "Copias disponibles")]
    public int CopiasDisponibles { get; set; } = 1;
}
