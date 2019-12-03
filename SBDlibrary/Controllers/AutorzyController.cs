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
    public class AutorzyController : Controller
    {
        private readonly LibraryDbContext _context;

        public AutorzyController (LibraryDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string nazwisko)
        {
            var autorzy = from m in _context.Autor
                         select m;

            if (!String.IsNullOrEmpty(nazwisko))
            {
                autorzy = autorzy.Where(s => s.nazwisko.Contains(nazwisko));
            }

            return View(autorzy);
        }

        public IActionResult Stworz()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Stworz([Bind("id_autor,imie,nazwisko")]Autor autor)
        {
            if (ModelState.IsValid)
            {
                var check = await _context.Autor.FirstOrDefaultAsync(m => m.imie.ToUpper() == autor.imie.ToUpper() 
                                                                    && m.nazwisko.ToUpper() == autor.nazwisko.ToUpper());
                if (check != null)
                {
                    ModelState.AddModelError("", "Autor o podanym imieniu i nazwisku istnieje w bazie");
                    return View(autor);
                }
                _context.Autor.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Autorzy");
            }

            return View(autor);
        }

        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor.FirstOrDefaultAsync(m => m.id_autor == id);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }  

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, [Bind("id_autor,imie,nazwisko")]Autor autor)
        {
            if (id != autor.id_autor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var check = await _context.Autor.FirstOrDefaultAsync(m => m.imie.ToUpper() == autor.imie.ToUpper()
                                                                    && m.nazwisko.ToUpper() == autor.nazwisko.ToUpper());
                if (check != null)
                {
                    ModelState.AddModelError("", "Autor o podanym imieniu i nazwisku istnieje w bazie");
                    return View(autor);
                }

                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.id_autor))
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

            return View(autor);
        }

        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor.FirstOrDefaultAsync(m => m.id_autor == id);
            
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Usun(int id)
        {
            var autor = await _context.Autor.FindAsync(id);
            _context.Autor.Remove(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _context.Autor.Any(e => e.id_autor == id);
        }
    }
}