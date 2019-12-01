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
    public class DostawcyController : Controller
    {
        private readonly LibraryDbContext _context;

        public DostawcyController (LibraryDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string nazwa)
        {
            var dostawcy = from m in _context.Dostawcy
                          select m;

            if (!String.IsNullOrEmpty(nazwa))
            {
                dostawcy = dostawcy.Where(s => s.nazwa.Contains(nazwa));
            }

            return View(dostawcy);
        }

        [HttpGet]
        public IActionResult Stworz()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Stworz([Bind("nazwa, adres")]Dostawcy dostawca)
        {
            if (ModelState.IsValid)
            {
                var check = await _context.Dostawcy.FirstOrDefaultAsync(m => m.nazwa.ToUpper() == dostawca.nazwa.ToUpper());
                if (check != null)
                {
                    ModelState.AddModelError("", "Dostawca o podanej nazwie istnieje w bazie");
                    return View(dostawca);
                }

                _context.Dostawcy.Add(dostawca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(dostawca);
        }

        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostawca = await _context.Dostawcy.FirstOrDefaultAsync(m => m.id_dostawcy == id);

            if (dostawca == null)
            {
                return NotFound();
            }

            return View(dostawca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, [Bind("id_dostawcy,nazwa,adres")]Dostawcy dostawca)
        {
            if (id != dostawca.id_dostawcy)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var check = await _context.Dostawcy.FirstOrDefaultAsync(m => m.nazwa.ToUpper() == dostawca.nazwa.ToUpper());
                if (check != null)
                {
                    ModelState.AddModelError("", "Dostawca o podanej nazwie istnieje w bazie");
                    return View(dostawca);
                }

                try
                {
                    _context.Update(dostawca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DostawcaExists(dostawca.id_dostawcy))
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

            return View(dostawca);
        }

        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostawca = await _context.Dostawcy.FirstOrDefaultAsync(m => m.id_dostawcy == id);

            if (dostawca == null)
            {
                return NotFound();
            }

            return View(dostawca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Usun(int id)
        {
            var dostawca = await _context.Dostawcy.FindAsync(id);
            _context.Dostawcy.Remove(dostawca);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DostawcaExists(int id)
        {
            return _context.Dostawcy.Any(e => e.id_dostawcy == id);
        }

    }
}