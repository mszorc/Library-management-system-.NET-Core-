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
    public class DostawcyController : Controller
    {
        private readonly LibraryDbContext _context;

        public DostawcyController (LibraryDbContext context)
        {
            _context = context;
        }

        public async  Task<IActionResult> Index()
        {
            return View(await _context.Dostawcy.ToListAsync());
           // return View();
        }

        public async Task<ActionResult> Details(int? id)
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
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("nazwa, adres")]Dostawcy dostawca)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Dostawcy.Add(dostawca);
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