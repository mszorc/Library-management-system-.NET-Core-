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
                ModelState.AddModelError(string.Empty, "Egzemplarz does not exist.");
                return View();
            }

            var egzemplarz = _context.Egzemplarze.FirstOrDefault(m => m.id_egzemplarza == id);
         //   egzemplarz.Ksiazki.id_ksiazki = a
            
            egzemplarz.Ksiazki = _context.Ksiazki.FirstOrDefault(m => m.id_ksiazki == egzemplarz.id_ksiazki);


            return View(egzemplarz);


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
        public async Task<ActionResult> Create()
        {
           
            return View(new SimpleCreateModel());
        }

    }
}