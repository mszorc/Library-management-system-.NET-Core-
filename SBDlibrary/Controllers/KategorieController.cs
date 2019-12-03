using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;

namespace SBDlibrary.Controllers
{
    [Authorize(Roles = "Bibliotekarz")]
    public class KategorieController : Controller
    {
        private readonly LibraryDbContext _context;

        public KategorieController(LibraryDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string nazwa)
        {
            var kategorie = from m in _context.Kategorie
                          select m;

            if (!String.IsNullOrEmpty(nazwa))
            {
                kategorie = kategorie.Where(s => s.nazwa.Contains(nazwa));
            }

            return View(kategorie);
        }

        [HttpGet]
        public IActionResult Stworz()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Stworz([Bind("nazwa")]Kategorie kategoria)
        {
            if (ModelState.IsValid)
            {
                var check = await _context.Kategorie.FirstOrDefaultAsync(m => m.nazwa == kategoria.nazwa);
                if (check != null)
                {
                    ModelState.AddModelError("", "Kategoria o podanej nazwie istnieje w bazie");
                    return View(kategoria);
                }

                _context.Kategorie.Add(kategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(kategoria);
        }

        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategoria = await _context.Kategorie.FirstOrDefaultAsync(m => m.id_kategorii == id);

            if (kategoria == null)
            {
                return NotFound();
            }

            return View(kategoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, [Bind("id_kategorii,nazwa")]Kategorie kategoria)
        {
            if (id != kategoria.id_kategorii)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var check = await _context.Kategorie.FirstOrDefaultAsync(m => m.nazwa.ToUpper() == kategoria.nazwa.ToUpper());
                if (check != null)
                {
                    ModelState.AddModelError("", "Kategoria o podanej nazwie istnieje w bazie");
                    return View(kategoria);
                }

                try
                {
                    _context.Update(kategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriaExists(kategoria.id_kategorii))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(kategoria);
        }

        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategoria = await _context.Kategorie.FirstOrDefaultAsync(m => m.id_kategorii == id);

            if (kategoria == null)
            {
                return NotFound();
            }

            return View(kategoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Usun(int id)
        {
            var kategoria = await _context.Kategorie.FindAsync(id);
            var kategoria_ksiazki = await _context.Kategorie_Ksiazki.FirstOrDefaultAsync(m => m.id_kategorii == id);
            if (kategoria_ksiazki != null)
            {
                ModelState.AddModelError("", "Nie można usunąć kategorii, ponieważ istnieją połączenia z książkami.");
                return View(kategoria);
            }
            _context.Kategorie.Remove(kategoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KategoriaExists(int id)
        {
            return _context.Kategorie.Any(e => e.id_kategorii == id);
        }
    }
}