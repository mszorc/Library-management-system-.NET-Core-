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
    public class AuthorController : Controller
    {

        private readonly LibraryDbContext _context;

        public AuthorController (LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Autor.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {           
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Autor.FirstOrDefaultAsync(m => m.id_autor == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("imie, nazwisko")]Autor autor)
        {
           // var wydawnictwo = await _context.Wydawnictwa
           //     .FirstOrDefaultAsync(m => m.id_wydawnictwa == 2);
         //   ksiazka.Wydawnictwa = wydawnictwo;


            //ksiazka.Wydawnictwa =await _context.Wydawnictwa.FindAsync(1);
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Autor.Add(autor);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Nie udało się dodać autora. Spróbuj ponownie.");
            }

            return RedirectToAction("Index", "Autor");
        }
       
    }

}