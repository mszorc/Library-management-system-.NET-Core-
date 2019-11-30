using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;

namespace SBDlibrary.Controllers
{
    public class AutorController : Controller
    {

        private readonly LibraryDbContext _context;

        public AutorController (LibraryDbContext context)
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

        public async Task<IActionResult> Usun(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor
                .FirstOrDefaultAsync(a => a.id_autor == id);
            if(autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        [HttpPost,ActionName("Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunConfirm (int id)
        {
            var autor = await _context.Autor.FindAsync(id);
            _context.Autor.Remove(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edytuj(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor
               .FirstOrDefaultAsync(a => a.id_autor == id);
            if (autor == null)                
            {
                return NotFound();
            }
            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id,Autor autor)
        {
            if(id != autor.id_autor)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.id_autor))
                    {
                        return NotFound();
                    }                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        private bool AutorExists(int id)
        {
            return _context.Autor.Any(a => a.id_autor == id);
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
                    await _context.SaveChangesAsync();
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