using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models;

public class Prestamo
{
    public int Id { get; set; }

    [Display(Name = "Libro")]
    public int LibroId { get; set; }
    public Libro? Libro { get; set; }

    [Display(Name = "Socio")]
    public int SocioId { get; set; }
    public Socio? Socio { get; set; }

    [Display(Name = "Fecha de préstamo")]
    public DateTime FechaPrestamo { get; set; } = DateTime.Today;

    [Display(Name = "Fecha de devolución")]
    public DateTime? FechaDevolucion { get; set; }
}
