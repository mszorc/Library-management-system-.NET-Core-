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

        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Kategoria does not exist.");
                return View();
            }

            var kategoria = _context.Kategorie.FirstOrDefault(m => m.id_kategorii == id);


            return View(kategoria);


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
                    _context.SaveChanges();
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
        public async Task<ActionResult> Create()
        {
            return View();
        }
    }
}