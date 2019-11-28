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
    public class WypozyczeniaController : Controller
    {
        private readonly LibraryDbContext _context;

        public WypozyczeniaController(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Wypozyczenia.ToListAsync());
        }
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var wypozyczenia = _context.Wypozyczenia.FirstOrDefault(m => m.id_wypozyczenia == id);

            if (wypozyczenia == null)
            {
                return NotFound();
            }
            wypozyczenia.Egzemplarze = _context.Egzemplarze.FirstOrDefault(m => m.id_egzemplarza == wypozyczenia.id_egzemplarza);
            wypozyczenia.Egzemplarze.Ksiazki = _context.Ksiazki.FirstOrDefault(m => m.id_ksiazki == wypozyczenia.Egzemplarze.id_ksiazki);
            return View(wypozyczenia);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new SimpleModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(string id_ksiazki)
        {
            int idKsiazki = Convert.ToInt32(id_ksiazki);
           // int idUzytkownika = Convert.ToInt32(id_uzytkownika);

         //   var uzytkownik = await _context.Uzytkownicy
             //    .FirstOrDefaultAsync(m => m.id_uzytkownika == idUzytkownika);

            var Ksiazka = await _context.Ksiazki
                .FirstOrDefaultAsync(m => m.id_ksiazki == idKsiazki);

            if (Ksiazka == null)
            {
                return NotFound();
            }

            var egzemplarze = from t in _context.Egzemplarze where t.Ksiazki.id_ksiazki == Ksiazka.id_ksiazki
                     select t;



            Egzemplarze dostepnyEgzemplarz = null;
            //var wypozyczenie = from b in egzemplarze from c in _context.Wypozyczenia.Where(c => b.id_egzemplarza == c.id_egzemplarza) select c;

            foreach (Egzemplarze x in egzemplarze)
            {

                if (x.status == "dostepny")
                {
                     dostepnyEgzemplarz = x;
                     break;
                }

            }

            Wypozyczenia wypozyczenie =new Wypozyczenia();
            wypozyczenie.Egzemplarze = dostepnyEgzemplarz;
            wypozyczenie.data_wypozyczenia = new DateTime();
            wypozyczenie.data_zwrotu = wypozyczenie.data_wypozyczenia.AddMonths(1);
            wypozyczenie.id_uzytkownika = 5;
   
            
            try
            {
                if (ModelState.IsValid)
                {
                    wypozyczenie.Egzemplarze.status = "wypozyczony";
                    _context.Wypozyczenia.Add(wypozyczenie);
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