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
    public class Kategorie_KsiazkiController : Controller
    {
        private readonly LibraryDbContext _context;
       
        public Kategorie_KsiazkiController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Kategorie_Ksiazki.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            //KatModel kat = new KatModel();
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Kategorie ksiazki does not exist.");
                return View();
            }
             

            var ksiazka = await _context.Ksiazki
               .FirstOrDefaultAsync(m => m.id_ksiazki == id);
            // int idKategorii = Convert.ToInt32(id_kategorii);

            var ka = from t in _context.Kategorie_Ksiazki
                     select t;
            ka = ka.Where(t => t.id_ksiazki.Equals(id));

            var kaka = from z in ka select z.Kategorie.nazwa;

          
            KatModel katmodel = new KatModel();
          //  katmodel.ksiazka = ksiazka;
            katmodel.kategorie = kaka;
            katmodel.tytul = ksiazka.tytuł;




         //   var kategoria = await _context.Kategorie_Ksiazki
          //      .(m => m.id_kategorii == id);

        //    var kat = await _context.Kategorie
           //     .AllAsync(m => m.id_kategorii == kategoria.id_kategorii);

        //    Kategorie_Ksiazki kategorie_Ksiazki = new Kategorie_Ksiazki();

        //    kategorie_Ksiazki.id_ksiazki = (int)id;
       //     kategorie_Ksiazki.id_kategorii = kategoria.id_kategorii;
       //     kategorie_Ksiazki.Kategorie = kat;
      //      kategorie_Ksiazki.Ksiazki = ksiazka;

          //  var kategrie = from t in _context.Kategorie_Ksiazki
                    //      select t;

            // var kategrie = _context.Kategorie_Ksiazki.All(m => m.id_ksiazki == id);

           // kategrie = kategrie.Where(t => t.id_ksiazki.Equals(id));

            //  kategrie = from t in kategrie select t.Kategorie;
           // kat.kategorie = from z in kategrie select z.Kategorie.nazwa;
           // kat.nazwa = "ello";

            //var xd = _context.Ksiazki.FirstOrDefaultAsync(m => m.id_ksiazki == id);
           /// string x = from z 

            
            
            return View(katmodel);


        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( string id_kategorii, string id_ksiazki)
        {
            Kategorie_Ksiazki kategorie_Ksiazki = new Kategorie_Ksiazki();
            int idKategorii = Convert.ToInt32(id_kategorii);
           // var kategoria = await _context.Kategorie
          //      .FirstOrDefaultAsync(m => m.id_kategorii == idKategorii);
            int idKsiazki = Convert.ToInt32(id_ksiazki);
          //  var ksiazka = await _context.Ksiazki
         //       .FirstOrDefaultAsync(m => m.id_ksiazki == idKsiazki);
            //   Debug.WriteLine("My debug string here" + idKsiazki);
           // kategorie_Ksiazki.id_kategorii = idKategorii;
            kategorie_Ksiazki.Ksiazki = await _context.Ksiazki
               .FirstOrDefaultAsync(p => p.id_ksiazki == idKsiazki);
          //  kategorie_Ksiazki.id_ksiazki = idKsiazki;
           kategorie_Ksiazki.Kategorie =  await _context.Kategorie
                .FirstOrDefaultAsync(m => m.id_kategorii == idKategorii);




            try
            {
                if (ModelState.IsValid)
                {
                    _context.Kategorie_Ksiazki.Add(kategorie_Ksiazki);
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