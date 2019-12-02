using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Bibliotekarz,Admin")]
        public async Task<IActionResult> Index(DateTime? DataOd, DateTime? DataDo, string imie, string nazwisko)
        {
           var wypozyczenia = from a in _context.Wypozyczenia select a;
            if (wypozyczenia != null)
            {
                foreach (Wypozyczenia x in wypozyczenia)
                {
                    x.Egzemplarze = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == x.id_egzemplarza);
                    x.Egzemplarze.Ksiazki = await _context.Ksiazki.FirstOrDefaultAsync(n => n.id_ksiazki == x.Egzemplarze.id_ksiazki);
                    x.Uzytkownicy = await _context.Uzytkownicy.FirstOrDefaultAsync(l => l.id_uzytkownika == x.id_uzytkownika);

                }

                if (DataOd != null)
                    wypozyczenia = wypozyczenia.Where(s => s.data_wypozyczenia >= DataOd);
                if (DataDo != null)
                    wypozyczenia = wypozyczenia.Where(s => s.data_wypozyczenia <= DataDo);
                if (!string.IsNullOrEmpty(imie))
                    wypozyczenia = wypozyczenia.Where(s => s.Uzytkownicy.imie.Contains(imie));
                if (!string.IsNullOrEmpty(nazwisko))
                    wypozyczenia = wypozyczenia.Where(s => s.Uzytkownicy.nazwisko.Contains(nazwisko));

            }

            return View(wypozyczenia);
        }
        public IActionResult Details(int? id)
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

       
        public async Task<ActionResult> Wypozycz(int? id_ksiazki)
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

                if (x.status == Egzemplarze.Status.Dostępny)
                {
                     dostepnyEgzemplarz = x;
                     break;
                }

            }

            Wypozyczenia wypozyczenie =new Wypozyczenia();
            wypozyczenie.Egzemplarze = dostepnyEgzemplarz;
            wypozyczenie.data_wypozyczenia = DateTime.UtcNow.Date.AddHours(1);
            wypozyczenie.data_zwrotu = wypozyczenie.data_wypozyczenia.AddMonths(1);
            wypozyczenie.id_uzytkownika = Convert.ToInt32(HttpContext.User.Identity.Name);
            


            try
            {
                if (ModelState.IsValid)
                {
                    wypozyczenie.Egzemplarze.status = Egzemplarze.Status.Wypozyczony;
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
        [Authorize(Roles = "Klient")]
        public async Task<IActionResult> KsiazkiKlienta(int? id)
        {
            // if(id==0)
            var idek = Convert.ToInt32(HttpContext.User.Identity.Name);
            var user = await _context.Uzytkownicy.FirstOrDefaultAsync(m => m.id_uzytkownika == idek);



            var wypozyczenia = _context.Wypozyczenia.Where(m => m.id_uzytkownika == user.id_uzytkownika);
            foreach(Wypozyczenia x in wypozyczenia)
            {
                x.Egzemplarze = await _context.Egzemplarze.FirstOrDefaultAsync(b => b.id_egzemplarza == x.id_egzemplarza);
                x.Egzemplarze.Ksiazki = await _context.Ksiazki.FirstOrDefaultAsync(c => c.id_ksiazki == x.Egzemplarze.id_ksiazki);
            }
          //  var egzemplarze = from b in wypozyczenia from c in _context.Egzemplarze.Where(c => b.id_egzemplarza == c.id_egzemplarza) select c;
         //   var Ksiazki = from c in egzemplarze from d in _context.Ksiazki.Where(d => c.id_ksiazki == d.id_ksiazki) select d;
            return View(wypozyczenia);
        }

    }
}