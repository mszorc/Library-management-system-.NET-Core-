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

        [Authorize(Roles = "Bibliotekarz")]
        public async Task<IActionResult> Index(string email)
        {
            var zwroty = await _context.Zwroty.ToListAsync();
            List<ZwrotKsiazkiViewModel> zwroty_vm = new List<ZwrotKsiazkiViewModel>();
            if (email == null) return View(zwroty_vm);
            var uzytkownicy = from m in _context.Uzytkownicy
                              select m;
            if (!String.IsNullOrEmpty(email))
            {
                uzytkownicy = uzytkownicy.Where(s => s.email.Contains(email));
            }

            foreach(var uzytkownik in uzytkownicy)
            {
                var wypozyczenia = await _context.Wypozyczenia.Where(m => m.id_uzytkownika == uzytkownik.id_uzytkownika).ToListAsync();

                foreach (var wypozyczenie in wypozyczenia)
                {
                    var zwrot = await _context.Zwroty.FirstOrDefaultAsync(m => m.id_wypozyczenia == wypozyczenie.id_wypozyczenia);
                    if (zwrot == null) continue;
                    var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == wypozyczenie.id_egzemplarza);
                    if (egzemplarz == null) return NotFound();
                    var ksiazka = await _context.Ksiazki.FirstOrDefaultAsync(m => m.id_ksiazki == egzemplarz.id_ksiazki);
                    if (ksiazka == null) return NotFound();
                    ZwrotKsiazkiViewModel temp = new ZwrotKsiazkiViewModel(zwrot.id_zwrotu, egzemplarz.id_egzemplarza, ksiazka.tytuł, wypozyczenie.id_wypozyczenia,
                        zwrot.data_zwrotu, (float)zwrot.kara, uzytkownik.email);
                    zwroty_vm.Add(temp);
                }
            }

            

            /*foreach (var zwrot in zwroty)
            {
                var wypozyczenie = await _context.Wypozyczenia.FirstOrDefaultAsync(m => m.id_wypozyczenia == zwrot.id_wypozyczenia);

                var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == wypozyczenie.id_egzemplarza);

                var ksiazka = await _context.Ksiazki.FirstOrDefaultAsync(m => m.id_ksiazki == egzemplarz.id_ksiazki);

                var user = await _userManager.FindByIdAsync((wypozyczenie.id_uzytkownika).ToString());

                ZwrotKsiazkiViewModel temp = new ZwrotKsiazkiViewModel(zwrot.id_zwrotu, egzemplarz.id_egzemplarza, ksiazka.tytuł, wypozyczenie.id_wypozyczenia,
                    zwrot.data_zwrotu, (float)zwrot.kara, user.email);

                zwroty_vm.Add(temp);
            }*/

            return View(zwroty_vm);
        }

        [Authorize(Roles = "Klient")]
        public async Task<IActionResult> ZwrotyKlienta()
        {
            var uzytkownik = await _userManager.GetUserAsync(User);
            var wypozyczenia = _context.Wypozyczenia.Where(m => m.id_uzytkownika == uzytkownik.id_uzytkownika);
            List<ZwrotKsiazkiViewModel> zwroty_vm = new List<ZwrotKsiazkiViewModel>();
            foreach (var wypozyczenie in wypozyczenia)
            {
                var zwrot = await _context.Zwroty.FirstOrDefaultAsync(m => m.id_wypozyczenia == wypozyczenie.id_wypozyczenia);
                if (zwrot != null)
                {
                    var egzemplarz = await _context.Egzemplarze.FirstOrDefaultAsync(m => m.id_egzemplarza == wypozyczenie.id_egzemplarza);
                    if (egzemplarz == null) return NotFound();
                    var ksiazka = await _context.Ksiazki.FirstOrDefaultAsync(m => m.id_ksiazki == egzemplarz.id_ksiazki);
                    if (ksiazka == null) return NotFound();
                    ZwrotKsiazkiViewModel temp = new ZwrotKsiazkiViewModel(zwrot.id_zwrotu, egzemplarz.id_egzemplarza, ksiazka.tytuł, wypozyczenie.id_wypozyczenia,
                            zwrot.data_zwrotu, (float)zwrot.kara, uzytkownik.email);
                    zwroty_vm.Add(temp);
                }
            }
            return View(zwroty_vm);
        }
      
    }
}
