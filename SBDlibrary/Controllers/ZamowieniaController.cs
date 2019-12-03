using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;
using SBDlibrary.ViewModels.ZamowieniaViewModels;

namespace SBDlibrary.Controllers
{
    [Authorize(Roles = "Bibliotekarz,Admin")]
    public class ZamowieniaController : Controller
    {
        private readonly LibraryDbContext _context;

        public ZamowieniaController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //dodac sortowanie
            var zamowienia = _context.Zamowienia.Include(z => z.dostawcy);
            
            return View(await zamowienia.ToListAsync());
        }

        [HttpPost]
        public IActionResult Index(DateTime DataOd, DateTime DataDo)
        {
            var zamowienia = _context.Zamowienia.Include(z => z.dostawcy);
            List<Zamowienia> zamowienia2 = new List<Zamowienia>();

            if (DataOd != null && DataDo != null)
            {
                foreach (var zamowienie in zamowienia)
                {
                    if (zamowienie.data_zamowienia >= DataOd && zamowienie.data_zamowienia <= DataDo)
                    {
                        zamowienia2.Add(zamowienie);
                    }
                }
            }
            return View(zamowienia2);
        }


        public async Task<IActionResult> Detale(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie_ksiazki = await _context.Zamowienie_ksiazki.Where(m => m.id_zamowienia == id).ToListAsync();
            var zamowienie = await _context.Zamowienia.FirstOrDefaultAsync(m => m.id_zamowienia == id);
            if (zamowienie == null)
            {
                return NotFound();
            }

            if (zamowienie.status_zamowienia == Zamowienia.Status.Zamówione)
            {
                ViewBag.Status = Zamowienia.Status.Zamówione;
            }

            List<ZamowienieKsiazkiViewModel> zamowienie_ksiazki_vm = new List<ZamowienieKsiazkiViewModel>();

            foreach (var ksiazka in zamowienie_ksiazki)
            {
                var tytul = await _context.Ksiazki.FirstOrDefaultAsync(m => m.id_ksiazki == ksiazka.id_ksiazki);
                var temp = new ZamowienieKsiazkiViewModel(ksiazka.id_zamowienia, ksiazka.id_ksiazki, tytul.tytuł, ksiazka.ilosc);
                zamowienie_ksiazki_vm.Add(temp);
            }
            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł");

            return View(zamowienie_ksiazki_vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detale(int id, [Bind("id_zamowienia,id_ksiazki,tytul,ilosc")] ZamowienieKsiazkiViewModel zamowienie_ksiazki)
        {
            if (zamowienie_ksiazki.ilosc > 1000 || zamowienie_ksiazki.ilosc < 1)
            {
                ModelState.AddModelError("", "Ilość książek musi mieścić się w przedziale 1-1000");
                return RedirectToAction("Detale", "Zamowienia", id);
            }
            if (ModelState.IsValid)
            {
                
                var temp = new Zamowienie_ksiazki();
                temp.id_zamowienia = id;
                temp.id_ksiazki = zamowienie_ksiazki.id_ksiazki;
                temp.ilosc = zamowienie_ksiazki.ilosc;
                var duplikat = await _context.Zamowienie_ksiazki.FirstOrDefaultAsync(m => m.id_zamowienia == id && m.id_ksiazki == temp.id_ksiazki);
                if (duplikat != null)
                {
                    duplikat.ilosc += temp.ilosc;
                    _context.Update(duplikat);
                }
                else
                {
                    _context.Add(temp);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Detale", "Zamowienia", new { id = id });
            }

            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł", zamowienie_ksiazki.id_ksiazki);
            return View(zamowienie_ksiazki.id_zamowienia);
        }
        
        [Authorize(Roles = "Bibliotekarz")]
        public IActionResult Stworz()
        {
            ViewData["id_dostawcy"] = new SelectList(_context.Dostawcy, "id_dostawcy", "adres");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Bibliotekarz")]
        public async Task<IActionResult> Stworz([Bind("id_dostawcy")] Zamowienia zamowienie)
        {
            zamowienie.data_zamowienia = DateTime.Now;
            zamowienie.status_zamowienia = Zamowienia.Status.Zamówione;
            _context.Add(zamowienie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienia
                .Include(z => z.dostawcy)
                .FirstOrDefaultAsync(m => m.id_zamowienia == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            if (zamowienie.status_zamowienia == Zamowienia.Status.Odebrane)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Usun(int id)
        {
            var zamowienie = await _context.Zamowienia.FindAsync(id);
            _context.Zamowienia.Remove(zamowienie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> UsunKsiazki(int? id_zamowienia, int? id_ksiazki)
        {
            if (id_zamowienia == null || id_ksiazki == null)
            {
                return NotFound();
            }

            var zamowienie_ksiazki = await _context.Zamowienie_ksiazki.FirstOrDefaultAsync(m => m.id_zamowienia == id_zamowienia
                                                                                && m.id_ksiazki == id_ksiazki);
            if (zamowienie_ksiazki == null)
            {
                return NotFound();
            }

            _context.Zamowienie_ksiazki.Remove(zamowienie_ksiazki);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detale", "Zamowienia", new { id = id_zamowienia });
        }

        public async Task<IActionResult> OdbierzZamowienie(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienia.FirstOrDefaultAsync(m => m.id_zamowienia == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            if (zamowienie.status_zamowienia == Zamowienia.Status.Odebrane)
            {
                return NotFound();
            }

            var zamowienie_ksiazki = await _context.Zamowienie_ksiazki.Where(m => m.id_zamowienia == zamowienie.id_zamowienia).ToListAsync();

            foreach(var book in zamowienie_ksiazki)
            {
                for (int i = 0; i < book.ilosc; i++)
                {
                    var egzemplarz = new Egzemplarze();
                    egzemplarz.id_ksiazki = book.id_ksiazki;
                    egzemplarz.status = Egzemplarze.Status.Dostępny;
                    _context.Add(egzemplarz);
                }
            }

            zamowienie.status_zamowienia = Zamowienia.Status.Odebrane;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
             

        private bool ZamowieniaExists(int id)
        {
            return _context.Zamowienia.Any(e => e.id_zamowienia == id);
        }
    }
}
