using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;
using SBDlibrary.ViewModels.WypozyczeniaViewModels;

namespace SBDlibrary.Controllers
{
    public class RezerwacjeController : Controller
    {

        private readonly LibraryDbContext _context;
        private readonly UserManager<Uzytkownicy> _userManager;
        private IHttpContextAccessor _accessor;

        public RezerwacjeController(LibraryDbContext context, UserManager<Uzytkownicy> userManager,
                            IHttpContextAccessor accessor)
        {
            _context = context;
            _userManager = userManager;
            _accessor = accessor;
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
        [Authorize(Roles = "Bibliotekarz")]
        public async Task<IActionResult> Index(DateTime ?DataOd, DateTime ?DataDo, string imie, string nazwisko)
        {
            var rezerwacje = from a in _context.Rezerwacje select a;
            if (rezerwacje != null)
            {
                foreach (Rezerwacje x in rezerwacje)
                {
                    x.Egzemplarze = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == x.id_egzemplarza);
                    x.Egzemplarze.Ksiazki = await _context.Ksiazki.FirstOrDefaultAsync(n => n.id_ksiazki == x.Egzemplarze.id_ksiazki);
                    x.Uzytkownicy = await _context.Uzytkownicy.FirstOrDefaultAsync(l => l.id_uzytkownika == x.id_uzytkownika);
                }

                if (DataOd != null)
                    rezerwacje = rezerwacje.Where(s => s.data_rezerwacji >= DataOd);
                if (DataDo != null)
                    rezerwacje = rezerwacje.Where(s => s.data_rezerwacji <= DataDo);
                if (!string.IsNullOrEmpty(imie))
                    rezerwacje = rezerwacje.Where(s => s.Uzytkownicy.imie.Contains(imie));
                if (!string.IsNullOrEmpty(nazwisko))
                    rezerwacje = rezerwacje.Where(s => s.Uzytkownicy.nazwisko.Contains(nazwisko));
            }

            return View(rezerwacje);
        }
        [Authorize(Roles = "Klient,Bibliotekarz,Admin")]
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
                    var uzytkownik = await _userManager.GetUserAsync(User);
                    _context.Logi.Add(stworzLog(uzytkownik, "stworzono rezerwację o numerze " + rezerwacja.id_rezerwacji));
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

        [Authorize(Roles = "Bibliotekarz")]
        public async Task<IActionResult> ZarezerwujKlientowi()
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
        [Authorize(Roles = "Bibliotekarz")]
        public async Task<IActionResult> ZarezerwujKlientowi([Bind("id_egzemplarza,id_uzytkownika")] Rezerwacje rezerwacja)
        {
            var czas = DateTime.Now;
            rezerwacja.data_rezerwacji = czas;
            rezerwacja.data_odbioru = czas.AddDays(2);
            rezerwacja.status_rezerwacji = Rezerwacje.Status.aktualna;
            /*if (ModelState.IsValid)
            {
                var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == rezerwacja.id_egzemplarza);
                if (egzemplarz == null)
                {
                    return NotFound();
                }
                egzemplarz.status = Egzemplarze.Status.Zarezerwowany;
                _context.Egzemplarze.Update(egzemplarz);
                _context.Rezerwacje.Add(rezerwacja);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }*/
            var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == rezerwacja.id_egzemplarza);
            if (egzemplarz == null)
            {
                return NotFound();
            }
            egzemplarz.status = Egzemplarze.Status.Zarezerwowany;
            _context.Egzemplarze.Update(egzemplarz);
            _context.Rezerwacje.Add(rezerwacja);
            await _context.SaveChangesAsync();
            var uzytkownik = await _userManager.GetUserAsync(User);
            _context.Logi.Add(stworzLog(uzytkownik, "stworzono rezerwację o numerze " + rezerwacja.id_rezerwacji));
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Klient,Bibliotekarz,Admin")]
        public async Task<IActionResult> AnulujRezerwacje(int id_rezerwacji)
        {
            var rezerwacja = await _context.Rezerwacje
                 .FirstOrDefaultAsync(m => m.id_rezerwacji == id_rezerwacji);
            var uzytkownik = await _userManager.GetUserAsync(User);
            if (HttpContext.User.IsInRole("Klient") && rezerwacja.id_uzytkownika != uzytkownik.id_uzytkownika)
            {
                return NotFound();
            }
                rezerwacja.Egzemplarze = await _context.Egzemplarze
                 .FirstOrDefaultAsync(m => m.id_egzemplarza == rezerwacja.id_egzemplarza);

            if(rezerwacja!=null && rezerwacja.Egzemplarze != null)
            {
                rezerwacja.status_rezerwacji = Rezerwacje.Status.odwołano;
                rezerwacja.Egzemplarze.status = Egzemplarze.Status.Dostępny;
                _context.SaveChanges();
                _context.Logi.Add(stworzLog(uzytkownik, "anulowano rezerwację o numerze " + rezerwacja.id_rezerwacji));
                _context.SaveChanges();
            }

            if (HttpContext.User.IsInRole("Admin") || HttpContext.User.IsInRole("Bibliotekarz"))
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


        [Authorize(Roles = "Klient")]
        public async Task<IActionResult> wypozyczZarezerwowanyEgzemplarz(int? id_rezerwacji)
        {
            var idU = Convert.ToInt32(HttpContext.User.Identity.Name);
            var user = await _context.Uzytkownicy.FirstOrDefaultAsync(m => m.id_uzytkownika == idU);
            var rezerwacja = await _context.Rezerwacje.FirstOrDefaultAsync(m => m.id_rezerwacji == id_rezerwacji);
            
            var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == rezerwacja.id_egzemplarza);
            if (egzemplarz != null && rezerwacja != null && user != null)
            {
                egzemplarz.status = Egzemplarze.Status.Wypozyczony;
                rezerwacja.status_rezerwacji = Rezerwacje.Status.odebrano;
                Wypozyczenia wypozyczenia = new Wypozyczenia();
                wypozyczenia.Egzemplarze = egzemplarz;
                wypozyczenia.Uzytkownicy = user;
                wypozyczenia.data_wypozyczenia = DateTime.UtcNow.AddHours(1);
                wypozyczenia.data_zwrotu = wypozyczenia.data_wypozyczenia.AddMonths(2);
                
                _context.Wypozyczenia.Add(wypozyczenia);
                _context.SaveChanges();
                _context.Logi.Add(stworzLog(user, "wypożyczono książkę z rezerwacji o numerze " + rezerwacja.id_rezerwacji));
                _context.SaveChanges();
            }
            else return NotFound();


           return  RedirectToAction(nameof(RezerwacjeKlienta));
        }

        private Logi stworzLog(Uzytkownicy user, string komunikat)
        {
            Logi log = new Logi();
            var ip = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            log.Uzytkownicy = user;
            log.ip_urzadzenia = ip;
            log.komunikat = komunikat;
            return log;
        }
    }
}
