using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;
using SBDlibrary.ViewModels.WypozyczeniaViewModels;

namespace SBDlibrary.Controllers
{
    public class WypozyczeniaController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly UserManager<Uzytkownicy> _userManager;
        private const double KaraZaDzien = 0.2;

        public WypozyczeniaController(LibraryDbContext context, UserManager<Uzytkownicy> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Bibliotekarz")]
        public async Task<IActionResult> Index(DateTime? DataOd, DateTime? DataDo, string imie, string nazwisko)
        {
            var wypozyczenia = from a in _context.Wypozyczenia select a;
            List<Wypozyczenia> wypozyczenia_aktualne = new List<Wypozyczenia>();

            if (DataOd != null)
                wypozyczenia = wypozyczenia.Where(s => s.data_wypozyczenia >= DataOd);
            if (DataDo != null)
                wypozyczenia = wypozyczenia.Where(s => s.data_wypozyczenia <= DataDo);
            if (!string.IsNullOrEmpty(imie))
                wypozyczenia = wypozyczenia.Where(s => s.Uzytkownicy.imie.Contains(imie));
            if (!string.IsNullOrEmpty(nazwisko))
                wypozyczenia = wypozyczenia.Where(s => s.Uzytkownicy.nazwisko.Contains(nazwisko));

            if (wypozyczenia != null)
            {
                foreach (Wypozyczenia x in wypozyczenia)
                {
                    x.Egzemplarze = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == x.id_egzemplarza);
                    x.Egzemplarze.Ksiazki = await _context.Ksiazki.FirstOrDefaultAsync(n => n.id_ksiazki == x.Egzemplarze.id_ksiazki);
                    x.Uzytkownicy = await _context.Uzytkownicy.FirstOrDefaultAsync(l => l.id_uzytkownika == x.id_uzytkownika);
                    var zwrot = await _context.Zwroty.FirstOrDefaultAsync(m => m.id_wypozyczenia == x.id_wypozyczenia);
                    if (zwrot == null) wypozyczenia_aktualne.Add(x);
                }

            }

            return View(wypozyczenia_aktualne);
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
        public async Task<IActionResult> Zwroc(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wypozyczenie = await _context.Wypozyczenia.FirstOrDefaultAsync(m => m.id_wypozyczenia == id);

            if (wypozyczenie == null)
            {
                return NotFound();
            }

            var uzytkownik = await _userManager.GetUserAsync(User);

            if (wypozyczenie.id_uzytkownika != uzytkownik.id_uzytkownika)
            {
                return NotFound();
            }

            var zwrot = await _context.Zwroty.FirstOrDefaultAsync(m => m.id_wypozyczenia == wypozyczenie.id_wypozyczenia);

            if (zwrot != null)
            {
                return NotFound();
            }

            var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == wypozyczenie.id_egzemplarza);
            egzemplarz.status = Egzemplarze.Status.Dostępny;
            zwrot = new Zwroty();
            zwrot.id_wypozyczenia = wypozyczenie.id_wypozyczenia;
            DateTime now = DateTime.Now;
            zwrot.data_zwrotu = now;
            if ((wypozyczenie.data_zwrotu - now).TotalDays > 0) zwrot.kara = (float)0;
            else zwrot.kara = (float)(Math.Truncate((now - wypozyczenie.data_zwrotu).TotalDays * KaraZaDzien * 100) / 100);
            _context.Zwroty.Add(zwrot);
            await _context.SaveChangesAsync();

            return RedirectToAction("KsiazkiKlienta", "Wypozyczenia");
        }


        [Authorize(Roles = "Bibliotekarz,Admin")]
        public async Task<IActionResult> ZwrocAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wypozyczenie = await _context.Wypozyczenia.FirstOrDefaultAsync(m => m.id_wypozyczenia == id);

            if (wypozyczenie == null)
            {
                return NotFound();
            }

            var zwrot = await _context.Zwroty.FirstOrDefaultAsync(m => m.id_wypozyczenia == wypozyczenie.id_wypozyczenia);

            if (zwrot != null)
            {
                return NotFound();
            }


            var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == wypozyczenie.id_egzemplarza);
            egzemplarz.status = Egzemplarze.Status.Dostępny;
            zwrot = new Zwroty();
            zwrot.id_wypozyczenia = wypozyczenie.id_wypozyczenia;
            DateTime now = DateTime.Now;
            zwrot.data_zwrotu = now;
            if ((wypozyczenie.data_zwrotu - now).TotalDays > 0) zwrot.kara = (float)0;
            else zwrot.kara = (float)(Math.Truncate((now - wypozyczenie.data_zwrotu).TotalDays * KaraZaDzien * 100) / 100);
            _context.Zwroty.Add(zwrot);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Bibliotekarz,Admin")]
        public async Task<IActionResult> PrzedluzWypozyczenieAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wypozyczenie = await _context.Wypozyczenia.FirstOrDefaultAsync(m => m.id_wypozyczenia == id);

            if (wypozyczenie == null)
            {
                return NotFound();
            }

            if ((wypozyczenie.data_zwrotu - wypozyczenie.data_wypozyczenia).TotalDays <= 31)
            {
                wypozyczenie.data_zwrotu = wypozyczenie.data_wypozyczenia.AddMonths(2);
                _context.Wypozyczenia.Update(wypozyczenie);
                await _context.SaveChangesAsync();
            }
            else
            {
                ModelState.AddModelError("", "Nie można już przedłużyć okresu wypożyczenia");
            }

            return RedirectToAction("Index", "Wypozyczenia");
            
        }

        [Authorize(Roles = "Bibliotekarz,Admin")]
        public async Task<IActionResult> WypozyczKlientowi()
        {
            var ksiazki = await _context.Ksiazki.ToListAsync();
            List<EgzemplarzViewModel> egzemplarze = new List<EgzemplarzViewModel>();
            foreach (var ksiazka in ksiazki)
            {
                var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_ksiazki == ksiazka.id_ksiazki && m.status == Egzemplarze.Status.Dostępny);
                if (egzemplarz != null)
                {
                    var egzemplarz_vm = new EgzemplarzViewModel(egzemplarz.id_egzemplarza, egzemplarz.id_ksiazki, ksiazka.tytuł);
                    egzemplarze.Add(egzemplarz_vm);
                }
            }
            ViewData["id_egzemplarza"] = new SelectList(egzemplarze, "id_egzemplarza", "tytul");
            var uzytkownicy = await _context.Uzytkownicy.ToListAsync();
            ViewData["id_uzytkownika"] = new SelectList(uzytkownicy, "id_uzytkownika", "email");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Bibliotekarz,Admin")]
        public async Task<IActionResult> WypozyczKlientowi([Bind("id_egzemplarza,id_uzytkownika")] Wypozyczenia wypozyczenie)
        {
            var czas = DateTime.Now;
            wypozyczenie.data_wypozyczenia = czas;
            wypozyczenie.data_zwrotu = czas.AddMonths(1);
            if (ModelState.IsValid)
            {
                var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == wypozyczenie.id_egzemplarza);
                if (egzemplarz == null)
                {
                    return NotFound();
                }
                egzemplarz.status = Egzemplarze.Status.Wypozyczony;
                _context.Egzemplarze.Update(egzemplarz);
                _context.Wypozyczenia.Add(wypozyczenie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "Klient")]
        public async Task<IActionResult> KsiazkiKlienta(int? id)
        {
            // if(id==0)
            var idek = Convert.ToInt32(HttpContext.User.Identity.Name);
            var user = await _context.Uzytkownicy.FirstOrDefaultAsync(m => m.id_uzytkownika == idek);



            var wypozyczenia = _context.Wypozyczenia.Where(m => m.id_uzytkownika == user.id_uzytkownika);
            List<Wypozyczenia> wypozyczenia_aktualne = new List<Wypozyczenia>();
            foreach(Wypozyczenia x in wypozyczenia)
            {
                x.Egzemplarze = await _context.Egzemplarze.FirstOrDefaultAsync(b => b.id_egzemplarza == x.id_egzemplarza);
                x.Egzemplarze.Ksiazki = await _context.Ksiazki.FirstOrDefaultAsync(c => c.id_ksiazki == x.Egzemplarze.id_ksiazki); 
                var zwrot = await _context.Zwroty.FirstOrDefaultAsync(m => m.id_wypozyczenia == x.id_wypozyczenia);
                if (zwrot == null) wypozyczenia_aktualne.Add(x);
            }
          //  var egzemplarze = from b in wypozyczenia from c in _context.Egzemplarze.Where(c => b.id_egzemplarza == c.id_egzemplarza) select c;
         //   var Ksiazki = from c in egzemplarze from d in _context.Ksiazki.Where(d => c.id_ksiazki == d.id_ksiazki) select d;
            return View(wypozyczenia_aktualne);
        }

       
        

        [Authorize(Roles = "Klient")]
        public async Task<IActionResult> PrzedluzWYpozyczenie(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wypozyczenie = await _context.Wypozyczenia.FirstOrDefaultAsync(m => m.id_wypozyczenia == id);

            if (wypozyczenie == null)
            {
                return NotFound();
            }

            var uzytkownik = await _userManager.GetUserAsync(User);

            if (uzytkownik.id_uzytkownika == wypozyczenie.id_uzytkownika)
            {
                if ((wypozyczenie.data_zwrotu - wypozyczenie.data_wypozyczenia).TotalDays <= 31)
                {
                    wypozyczenie.data_zwrotu = wypozyczenie.data_wypozyczenia.AddMonths(2);
                    _context.Wypozyczenia.Update(wypozyczenie);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("", "Nie można już przedłużyć okresu wypożyczenia");
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("KsiazkiKlienta", "Wypozyczenia");
        }

        public IActionResult Wypozyczono()
        {
            return View();
        }
    }
}