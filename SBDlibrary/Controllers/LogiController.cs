using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;

namespace SBDlibrary.Controllers
{
    [Authorize(Roles="Admin")]
    public class LogiController : Controller
    {
        private readonly LibraryDbContext _context;

        public LogiController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string email)
        {
            var logi = from m in _context.Logi
                          select m;

            if (!String.IsNullOrEmpty(email))
            {
                var uzytkownik = await _context.Uzytkownicy.FirstOrDefaultAsync(m => m.email == email);
                if (uzytkownik != null)
                {
                    logi = logi.Where(s => s.id_uzytkownika == uzytkownik.id_uzytkownika);
                }
                    
            }

            return View(logi);
        }
    }
}