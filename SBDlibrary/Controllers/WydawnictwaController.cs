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
    [Authorize(Roles = "Bibliotekarz,Admin")]
    public class WydawnictwaController : Controller
    {
        private readonly LibraryDbContext _context;
        public WydawnictwaController(LibraryDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string nazwa)
        {
            var wydawnictwa = from m in _context.Wydawnictwa
                          select m;

            if (!String.IsNullOrEmpty(nazwa))
            {
                wydawnictwa = wydawnictwa.Where(s => s.nazwa.Contains(nazwa));
            }

            return View(wydawnictwa);
        }

        [HttpGet]
        public IActionResult Stworz()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Stworz([Bind("id_wydawnictwa,nazwa")]Wydawnictwa wydawnictwo)
        {
            if (ModelState.IsValid)
            {
                var check = await _context.Wydawnictwa.FirstOrDefaultAsync(m => m.nazwa.ToUpper() == wydawnictwo.nazwa.ToUpper());
                if (check != null)
                {
                    ModelState.AddModelError("", "Wydawnictwo o podanej nazwie istnieje w bazie");
                    return View(wydawnictwo);
                }
                _context.Wydawnictwa.Add(wydawnictwo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(wydawnictwo);
        }

        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wydawnictwo = await _context.Wydawnictwa.FirstOrDefaultAsync(m => m.id_wydawnictwa == id);

            if (wydawnictwo == null)
            {
                return NotFound();
            }

            return View(wydawnictwo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, [Bind("id_wydawnictwa,nazwa")]Wydawnictwa wydawnictwo)
        {
            if (id != wydawnictwo.id_wydawnictwa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var check = await _context.Wydawnictwa.FirstOrDefaultAsync(m => m.nazwa.ToUpper() == wydawnictwo.nazwa.ToUpper());
                if (check != null)
                {
                    ModelState.AddModelError("", "Wydawnictwo o podanej nazwie istnieje w bazie");
                    return View(wydawnictwo);
                }

                try
                {
                    _context.Update(wydawnictwo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WydawnictwoExists(wydawnictwo.id_wydawnictwa))
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

            return View(wydawnictwo);
        }

        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wydawnictwo = await _context.Wydawnictwa.FirstOrDefaultAsync(m => m.id_wydawnictwa == id);

            if (wydawnictwo == null)
            {
                return NotFound();
            }

            return View(wydawnictwo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Usun(int id)
        {
            var wydawnictwo = await _context.Wydawnictwa.FindAsync(id);
            _context.Wydawnictwa.Remove(wydawnictwo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WydawnictwoExists(int id)
        {
            return _context.Wydawnictwa.Any(e => e.id_wydawnictwa == id);
        }

    }

}
