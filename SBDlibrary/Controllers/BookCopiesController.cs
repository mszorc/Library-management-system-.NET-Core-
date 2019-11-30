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
    public class BookCopiesController : Controller
    {
        private readonly LibraryDbContext _context;

        public BookCopiesController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: BookCopies
        public async Task<IActionResult> Index()
        {
            var libraryDbContext = _context.Egzemplarze.Include(e => e.Ksiazki);
            return View(await libraryDbContext.ToListAsync());
        }

        // GET: BookCopies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egzemplarze = await _context.Egzemplarze
                .Include(e => e.Ksiazki)
                .FirstOrDefaultAsync(m => m.id_egzemplarza == id);
            if (egzemplarze == null)
            {
                return NotFound();
            }

            return View(egzemplarze);
        }

        // GET: BookCopies/Create
        public IActionResult Create()
        {
            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł");
            return View();
        }

        // POST: BookCopies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_egzemplarza,id_ksiazki,status")] Egzemplarze egzemplarze)
        {
            if (ModelState.IsValid)
            {
                _context.Add(egzemplarze);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł", egzemplarze.id_ksiazki);
            return View(egzemplarze);
        }

        // GET: BookCopies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egzemplarze = await _context.Egzemplarze.FindAsync(id);
            if (egzemplarze == null)
            {
                return NotFound();
            }
            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł", egzemplarze.id_ksiazki);
            return View(egzemplarze);
        }

        // POST: BookCopies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_egzemplarza,id_ksiazki,status")] Egzemplarze egzemplarze)
        {
            if (id != egzemplarze.id_egzemplarza)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(egzemplarze);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EgzemplarzeExists(egzemplarze.id_egzemplarza))
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
            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł", egzemplarze.id_ksiazki);
            return View(egzemplarze);
        }

        // GET: BookCopies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egzemplarze = await _context.Egzemplarze
                .Include(e => e.Ksiazki)
                .FirstOrDefaultAsync(m => m.id_egzemplarza == id);
            if (egzemplarze == null)
            {
                return NotFound();
            }

            return View(egzemplarze);
        }

        // POST: BookCopies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var egzemplarze = await _context.Egzemplarze.FindAsync(id);
            _context.Egzemplarze.Remove(egzemplarze);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EgzemplarzeExists(int id)
        {
            return _context.Egzemplarze.Any(e => e.id_egzemplarza == id);
        }
    }
}
