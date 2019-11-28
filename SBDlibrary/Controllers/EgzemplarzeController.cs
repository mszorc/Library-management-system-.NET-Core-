using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;

namespace SBDlibrary.Controllers
{
    public class EgzemplarzeController : Controller
    {
        private readonly LibraryDbContext _context;

        public EgzemplarzeController(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Egzemplarze.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == id);
         //   egzemplarz.Ksiazki.id_ksiazki = a
            
            egzemplarz.Ksiazki = await _context.Ksiazki.FirstOrDefaultAsync(m => m.id_ksiazki == egzemplarz.id_ksiazki);

            if (egzemplarz == null)
            {
                return NotFound();
            }

            return View(egzemplarz);


        }

        [HttpGet]
        public IActionResult Create()
        {

            return View(new SimpleCreateModel());
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string id_ksiazki)
        {
            int idKsiazki = Convert.ToInt32(id_ksiazki);
            Egzemplarze egzemplarz = new Egzemplarze();
            var ksiazka = await _context.Ksiazki
                .FirstOrDefaultAsync(m => m.id_ksiazki == idKsiazki);
         //   Debug.WriteLine("My debug string here" + idKsiazki);
            egzemplarz.Ksiazki = ksiazka;
            egzemplarz.status = "dostepny";

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Egzemplarze.Add(egzemplarz);
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
        

    }
}