using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers;

public class PrestamosController : Controller
{
    private readonly BibliotecaContext _db;
    public PrestamosController(BibliotecaContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Prestamos.Include(p => p.Libro).Include(p => p.Socio)
            .OrderByDescending(p => p.FechaPrestamo).ToListAsync());

    public async Task<IActionResult> Create()
    {
        await CargarListas();
        return View(new Prestamo());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Prestamo prestamo)
    {
        var libro = await _db.Libros.FindAsync(prestamo.LibroId);
        if (libro == null || libro.CopiasDisponibles < 1)
            ModelState.AddModelError("LibroId", "No hay copias disponibles de este libro.");

        if (!ModelState.IsValid)
        {
            await CargarListas();
            return View(prestamo);
        }

        libro!.CopiasDisponibles--;
        prestamo.FechaDevolucion = null;
        _db.Add(prestamo);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Devolver(int id)
    {
        var p = await _db.Prestamos.Include(x => x.Libro).FirstOrDefaultAsync(x => x.Id == id);
        if (p != null && p.FechaDevolucion == null)
        {
            p.FechaDevolucion = DateTime.Today;
            if (p.Libro != null) p.Libro.CopiasDisponibles++;
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListas()
    {
        ViewBag.Libros = new SelectList(
            await _db.Libros.Where(l => l.CopiasDisponibles > 0).OrderBy(l => l.Titulo).ToListAsync(),
            "Id", "Titulo");
        ViewBag.Socios = new SelectList(await _db.Socios.OrderBy(s => s.Nombre).ToListAsync(), "Id", "Nombre");
    }
}
