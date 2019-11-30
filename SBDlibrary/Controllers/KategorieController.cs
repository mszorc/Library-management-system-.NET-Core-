using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;

namespace SBDlibrary.Controllers
{
    public class KategorieController : Controller
    {
        private readonly LibraryDbContext _context;

        public KategorieController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Kategorie.ToListAsync());
            // return View();
        }
        
        public IActionResult Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var kategoria = _context.Kategorie.FirstOrDefault(m => m.id_kategorii == id);

            if (kategoria == null)
            {
                return NotFound();
            }

            return View(kategoria);


        }

        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategoria = await _context.Kategorie
                .FirstOrDefaultAsync(a => a.id_kategorii == id);
            if (kategoria == null)
            {
                return NotFound();
            }


            return View(kategoria);
        }

        [HttpPost, ActionName("Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunConfirm(int id)
        {
            var kategoria = await _context.Kategorie.FindAsync(id);
            _context.Kategorie.Remove(kategoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("nazwa")]Kategorie kategoria)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Kategorie.Add(kategoria);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return RedirectToAction("Index");
        }
    }
}