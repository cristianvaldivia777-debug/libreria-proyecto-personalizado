using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers;

public class HomeController : Controller
{
    private readonly BibliotecaContext _db;
    public HomeController(BibliotecaContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.Libros = await _db.Libros.CountAsync();
        ViewBag.Socios = await _db.Socios.CountAsync();
        ViewBag.Activos = await _db.Prestamos.CountAsync(p => p.FechaDevolucion == null);
        return View();
    }
}
