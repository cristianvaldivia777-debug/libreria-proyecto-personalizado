using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers;

public class SociosController : Controller
{
    private readonly BibliotecaContext _db;
    public SociosController(BibliotecaContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Socios.OrderBy(s => s.Nombre).ToListAsync());

    public IActionResult Create() => View(new Socio());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Socio socio)
    {
        if (!ModelState.IsValid) return View(socio);
        _db.Add(socio);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var socio = await _db.Socios.FindAsync(id);
        return socio == null ? NotFound() : View(socio);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Socio socio)
    {
        if (id != socio.Id) return NotFound();
        if (!ModelState.IsValid) return View(socio);
        _db.Update(socio);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var socio = await _db.Socios.FindAsync(id);
        return socio == null ? NotFound() : View(socio);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var socio = await _db.Socios.FindAsync(id);
        if (socio != null)
        {
            if (await _db.Prestamos.AnyAsync(p => p.SocioId == id))
            {
                TempData["Error"] = "No se puede eliminar: el socio tiene préstamos registrados.";
                return RedirectToAction(nameof(Index));
            }
            _db.Socios.Remove(socio);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
