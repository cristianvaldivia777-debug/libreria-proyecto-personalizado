using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models;

public class Socio
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio"), StringLength(100)]
    public string Nombre { get; set; } = "";

    [Required, EmailAddress(ErrorMessage = "Correo inválido")]
    public string Correo { get; set; } = "";

    [Display(Name = "Fecha de registro")]
    [DataType(DataType.Date)]
    public DateTime FechaRegistro { get; set; } = DateTime.Today;
}
