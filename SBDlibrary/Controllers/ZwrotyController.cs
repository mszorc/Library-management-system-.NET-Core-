using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;
using SBDlibrary.ViewModels.ZwrotyViewModels;

namespace SBDlibrary.Controllers
{
    [Authorize]
    public class ZwrotyController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly UserManager<Uzytkownicy> _userManager;
        private const float karaZaDzien = 0.2f;

        public ZwrotyController(LibraryDbContext context, UserManager<Uzytkownicy> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var wypozyczenia = await _context.Wypozyczenia.ToListAsync();
            foreach(var wypozyczenie in wypozyczenia)
            {

            }
            return NotFound();
        }

        // GET: Zwroty
        public async Task<IActionResult> ZwrotyKlienta()
        {
            var uzytkownik = await _userManager.GetUserAsync(User);
            var wypozyczenia = _context.Wypozyczenia.Where(m => m.id_uzytkownika == uzytkownik.id_uzytkownika);
            List<ZwrotKsiazkiViewModel> temp = new List<ZwrotKsiazkiViewModel>();
            foreach (var wypozyczenie in wypozyczenia)
            {
                var zwrot = await _context.Zwroty.FirstOrDefaultAsync(m => m.id_wypozyczenia == wypozyczenie.id_wypozyczenia);
                if (zwrot != null)
                {
                    var egz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == wypozyczenie.id_egzemplarza);
                    if (egz == null) return NotFound();
                    var ksiazka = await _context.Ksiazki.FirstOrDefaultAsync(m => m.id_ksiazki == egz.id_ksiazki);
                    if (ksiazka == null) return NotFound();
                    var x = new ZwrotKsiazkiViewModel(zwrot.id_zwrotu, wypozyczenie.id_egzemplarza, ksiazka.tytuł, wypozyczenie.id_wypozyczenia, DateTime.Now, zwrot.kara);
                    temp.Add(x);
                }
            }
            return View(temp);
        }

        // GET: Zwroty/Create
        public async Task<IActionResult> Zwroc()
        {
            var uzytkownik = await _userManager.GetUserAsync(User);
            var wypozyczenia = _context.Wypozyczenia.Where(m => m.id_uzytkownika == uzytkownik.id_uzytkownika);
            List<KsiazkaTytulViewModel> temp = new List<KsiazkaTytulViewModel>();
            foreach (var wypozyczenie in wypozyczenia)
            {
                var zwrot = await _context.Zwroty.FirstOrDefaultAsync(m => m.id_wypozyczenia == wypozyczenie.id_wypozyczenia);
                if (zwrot == null)
                {
                    var egz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == wypozyczenie.id_egzemplarza);
                    if (egz == null) return NotFound();
                    var ksiazka = await _context.Ksiazki.FirstOrDefaultAsync(m => m.id_ksiazki == egz.id_ksiazki);
                    if (ksiazka == null) return NotFound();
                    var x = new KsiazkaTytulViewModel(wypozyczenie.id_wypozyczenia, ksiazka.id_ksiazki, ksiazka.tytuł);
                    temp.Add(x);
                }
            }
            ViewData["id_wypozyczenia"] = new SelectList(temp, "id_wypozyczenia", "tytul");

            return View();
        }

        // POST: Zwroty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Zwroc([Bind("id_wypozyczenia,id_ksiazki,tytul")] KsiazkaTytulViewModel zwroty)
        {
            if (ModelState.IsValid)
            {
                var wypozyczenie = await _context.Wypozyczenia.FirstOrDefaultAsync(m => m.id_wypozyczenia == zwroty.id_wypozyczenia);
                var zwrot = new Zwroty();
                zwrot.id_wypozyczenia = zwroty.id_wypozyczenia;
                if ((wypozyczenie.data_zwrotu - DateTime.Now).TotalDays > 0) zwrot.kara = (float)0;
                else zwrot.kara = (float)((wypozyczenie.data_zwrotu - DateTime.Now).TotalDays * karaZaDzien);
                _context.Add(zwrot);
                await _context.SaveChangesAsync();
                return RedirectToAction("ZwrotyKlienta", "Zwroty");
            }
            ViewData["id_wypozyczenia"] = new SelectList(_context.Wypozyczenia, "id_wypozyczenia", "id_wypozyczenia", zwroty.id_wypozyczenia);
            return View(zwroty);
        }


        private bool ZwrotyExists(int id)
        {
            return _context.Zwroty.Any(e => e.id_zwrotu == id);
        }
    }
}
