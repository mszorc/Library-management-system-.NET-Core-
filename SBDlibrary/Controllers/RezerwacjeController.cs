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
    public class RezerwacjeController : Controller
    {

        private readonly LibraryDbContext _context;

        public RezerwacjeController(LibraryDbContext context)
        {
            _context = context;
        }
        //[Authorize(Roles = "Bibliotekarz,Admin")]
        
        //public async Task<IActionResult> Index()
        //{
        //    var rezerwacje = from a in _context.Rezerwacje select a;
        //    foreach (Rezerwacje x in rezerwacje)
        //    {
        //        x.Egzemplarze = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == x.id_egzemplarza);
        //        x.Egzemplarze.Ksiazki = await _context.Ksiazki.FirstOrDefaultAsync(n => n.id_ksiazki == x.Egzemplarze.id_ksiazki);
        //        x.Uzytkownicy = await _context.Uzytkownicy.FirstOrDefaultAsync(l => l.id_uzytkownika == x.id_uzytkownika);
        //    }
        //    return View(rezerwacje);
        //}
        //[HttpPost]
        [Authorize(Roles = "Bibliotekarz,Admin")]
        public async Task<IActionResult> Index(DateTime DataOd, DateTime DataDo, string imie, string nazwisko,Rezerwacje.Status status)
        {
            var rezerwacje = from a in _context.Rezerwacje select a;
            foreach (Rezerwacje x in rezerwacje)
            {
                x.Egzemplarze = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == x.id_egzemplarza);
                x.Egzemplarze.Ksiazki = await _context.Ksiazki.FirstOrDefaultAsync(n => n.id_ksiazki == x.Egzemplarze.id_ksiazki);
                x.Uzytkownicy = await _context.Uzytkownicy.FirstOrDefaultAsync(l => l.id_uzytkownika == x.id_uzytkownika);
            }

            if(DataOd !=null)
                rezerwacje = rezerwacje.Where(s => s.data_rezerwacji>=DataOd);
            if (DataDo != null)
                rezerwacje = rezerwacje.Where(s => s.data_rezerwacji <= DataDo);
            if (!string.IsNullOrEmpty(imie))
                rezerwacje = rezerwacje.Where(s => s.Uzytkownicy.imie.Contains(imie));
            if (!string.IsNullOrEmpty(nazwisko))
                rezerwacje = rezerwacje.Where(s => s.Uzytkownicy.nazwisko.Contains(nazwisko));
            

            return View(rezerwacje);
        }

        public async Task<IActionResult> Zarezerwuj(int id_ksiazki)
        {
            var Ksiazka = await _context.Ksiazki
                 .FirstOrDefaultAsync(m => m.id_ksiazki == id_ksiazki);

            if (Ksiazka == null)
            {
                return NotFound();
            }

            var egzemplarze = from t in _context.Egzemplarze
                              where t.Ksiazki.id_ksiazki == Ksiazka.id_ksiazki
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

            Rezerwacje rezerwacja = new Rezerwacje();
            rezerwacja.Egzemplarze = dostepnyEgzemplarz;
            rezerwacja.data_rezerwacji = DateTime.UtcNow.Date.AddHours(1);
            rezerwacja.data_odbioru = rezerwacja.data_rezerwacji.AddDays(2);
            rezerwacja.id_uzytkownika = Convert.ToInt32(HttpContext.User.Identity.Name);




            try
            {
                if (ModelState.IsValid)
                {
                    rezerwacja.status_rezerwacji = Rezerwacje.Status.aktualna;
                    rezerwacja.Egzemplarze.status = Egzemplarze.Status.Zarezerwowany;
                    _context.Rezerwacje.Add(rezerwacja);
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

        public async Task<IActionResult> AnulujRezerwacje(int id_rezerwacji)
        {
            var rezerwacja = await _context.Rezerwacje
                 .FirstOrDefaultAsync(m => m.id_rezerwacji == id_rezerwacji);
            rezerwacja.Egzemplarze = await _context.Egzemplarze
                 .FirstOrDefaultAsync(m => m.id_egzemplarza == rezerwacja.id_egzemplarza);

            if(rezerwacja!=null && rezerwacja.Egzemplarze != null)
            {
                rezerwacja.status_rezerwacji = Rezerwacje.Status.odwołano;
                rezerwacja.Egzemplarze.status = Egzemplarze.Status.Dostępny;
                _context.SaveChanges();
            }

            if (HttpContext.User.IsInRole("Administrator") || HttpContext.User.IsInRole("Bibliotekarz"))
            {
                return RedirectToAction(nameof(Index));
            }
            else
                return RedirectToAction(nameof(RezerwacjeKlienta));
        }
      
        
        [Authorize(Roles = "Klient")]
        public async Task<IActionResult> RezerwacjeKlienta(int? id)
        {
            var idek = Convert.ToInt32(HttpContext.User.Identity.Name);

            
            var user = await _context.Uzytkownicy.FirstOrDefaultAsync(m => m.id_uzytkownika == idek);

            var rezerwacje = _context.Rezerwacje.Where(m => m.id_uzytkownika == user.id_uzytkownika);
            foreach (Rezerwacje x in rezerwacje)
            {
                x.Egzemplarze = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == x.id_egzemplarza);
                x.Uzytkownicy = await _context.Uzytkownicy.FirstOrDefaultAsync(m => m.id_uzytkownika == x.id_uzytkownika);
            }
            return View(rezerwacje);
        }
    }





}
