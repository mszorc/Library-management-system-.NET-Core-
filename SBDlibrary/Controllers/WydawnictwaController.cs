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
    public class WydawnictwaController : Controller
    {
        private readonly LibraryDbContext _context;
        public WydawnictwaController(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Wydawnictwa.ToListAsync());
        }
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var Wydawnictwo = _context.Wydawnictwa.FirstOrDefault(m => m.id_wydawnictwa == id);

            if (Wydawnictwo == null)
            {
               return NotFound();
            }
            return View(Wydawnictwo);

            // return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("nazwa")]Wydawnictwa wydawnictwo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Wydawnictwa.Add(wydawnictwo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(wydawnictwo);
        }

    }

}
