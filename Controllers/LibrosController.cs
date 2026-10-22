using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers;

public class LibrosController : Controller
{
    private readonly BibliotecaContext _db;
    public LibrosController(BibliotecaContext db) => _db = db;

    public async Task<IActionResult> Index(string? buscar)
    {
        var q = _db.Libros.AsQueryable();
        if (!string.IsNullOrWhiteSpace(buscar))
            q = q.Where(l => l.Titulo.Contains(buscar) || l.Autor.Contains(buscar));
        ViewBag.Buscar = buscar;
        return View(await q.OrderBy(l => l.Titulo).ToListAsync());
    }

    public IActionResult Create() => View(new Libro());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        if (!ModelState.IsValid) return View(libro);
        _db.Add(libro);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var libro = await _db.Libros.FindAsync(id);
        return libro == null ? NotFound() : View(libro);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Libro libro)
    {
        if (id != libro.Id) return NotFound();
        if (!ModelState.IsValid) return View(libro);
        _db.Update(libro);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var libro = await _db.Libros.FindAsync(id);
        return libro == null ? NotFound() : View(libro);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var libro = await _db.Libros.FindAsync(id);
        if (libro != null)
        {
            if (await _db.Prestamos.AnyAsync(p => p.LibroId == id))
            {
                TempData["Error"] = "No se puede eliminar: el libro tiene préstamos registrados.";
                return RedirectToAction(nameof(Index));
            }
            _db.Libros.Remove(libro);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
