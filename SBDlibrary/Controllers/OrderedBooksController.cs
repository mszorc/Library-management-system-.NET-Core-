using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;

namespace SBDlibrary.Controllers
{
    public class OrderedBooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public OrderedBooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GEEgzemplarzesT: OrderedBooks
        public async Task<IActionResult> Index()
        {
            var libraryDbContext = _context.Zamowienie_ksiazki.Include(z => z.Ksiazki).Include(z => z.Zamowienia);
            return View(await libraryDbContext.ToListAsync());
        }

        // GET: OrderedBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie_ksiazki = await _context.Zamowienie_ksiazki
                .Include(z => z.Ksiazki)
                .Include(z => z.Zamowienia)
                .FirstOrDefaultAsync(m => m.id_zamowienia == id);
            if (zamowienie_ksiazki == null)
            {
                return NotFound();
            }

            return View(zamowienie_ksiazki);
        }

        // GET: OrderedBooks/Create
        public IActionResult Create()
        {
            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł");
            ViewData["id_zamowienia"] = new SelectList(_context.Zamowienia, "id_zamowienia", "id_zamowienia");
            return View();
        }

        // POST: OrderedBooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_zamowienia,id_ksiazki,ilosc")] Zamowienie_ksiazki zamowienie_ksiazki)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zamowienie_ksiazki);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł", zamowienie_ksiazki.id_ksiazki);
            ViewData["id_zamowienia"] = new SelectList(_context.Zamowienia, "id_zamowienia", "id_zamowienia", zamowienie_ksiazki.id_zamowienia);
            return View(zamowienie_ksiazki);
        }

        // GET: OrderedBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie_ksiazki = await _context.Zamowienie_ksiazki.FindAsync(id);
            if (zamowienie_ksiazki == null)
            {
                return NotFound();
            }
            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł", zamowienie_ksiazki.id_ksiazki);
            ViewData["id_zamowienia"] = new SelectList(_context.Zamowienia, "id_zamowienia", "id_zamowienia", zamowienie_ksiazki.id_zamowienia);
            return View(zamowienie_ksiazki);
        }

        // POST: OrderedBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_zamowienia,id_ksiazki,ilosc")] Zamowienie_ksiazki zamowienie_ksiazki)
        {
            if (id != zamowienie_ksiazki.id_zamowienia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zamowienie_ksiazki);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Zamowienie_ksiazkiExists(zamowienie_ksiazki.id_zamowienia))
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
            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł", zamowienie_ksiazki.id_ksiazki);
            ViewData["id_zamowienia"] = new SelectList(_context.Zamowienia, "id_zamowienia", "id_zamowienia", zamowienie_ksiazki.id_zamowienia);
            return View(zamowienie_ksiazki);
        }

        // GET: OrderedBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie_ksiazki = await _context.Zamowienie_ksiazki
                .Include(z => z.Ksiazki)
                .Include(z => z.Zamowienia)
                .FirstOrDefaultAsync(m => m.id_zamowienia == id);
            if (zamowienie_ksiazki == null)
            {
                return NotFound();
            }

            return View(zamowienie_ksiazki);
        }

        // POST: OrderedBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zamowienie_ksiazki = await _context.Zamowienie_ksiazki.FindAsync(id);
            _context.Zamowienie_ksiazki.Remove(zamowienie_ksiazki);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Zamowienie_ksiazkiExists(int id)
        {
            return _context.Zamowienie_ksiazki.Any(e => e.id_zamowienia == id);
        }
    }
}
