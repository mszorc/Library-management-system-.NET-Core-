using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;
using SBDlibrary.ViewModels.BookViewModels;

namespace SBDlibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly  LibraryDbContext _context;


        public BookController(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
           
            // if (!HttpContext.User.IsInRole(""))
            // {

            //  }
            return View(await _context.Ksiazki.ToListAsync());
            //return View();
        }

        public async Task<ActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var ksiazka = await _context.Ksiazki.FirstOrDefaultAsync(m => m.id_ksiazki == id);

            if (ksiazka == null)
            {
                return NotFound();
            }

            //if (ksiazka == null)
            //{
            //    ModelState.AddModelError(string.Empty, "Nie ma!.");
            //    return View();
            //}
            return View(ksiazka);

           // return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Kategorie> kategorieList = await _context.Kategorie.ToListAsync();
            IQueryable<string> kategorieQuery = from k in _context.Kategorie orderby k.nazwa select k.nazwa;

            var kategorieVM = new KsiazkaViewModel
            {
                kategorieListVM = kategorieList,
                kategorie = new SelectList(await kategorieQuery.Distinct().ToListAsync())
            };
            return View(kategorieVM);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("tytuł, data_wydania")]KsiazkaViewModel ksiazkaVM)
        {
            ksiazkaVM.Wydawnictwa = await _context.Wydawnictwa
                .FirstOrDefaultAsync(m => m.id_wydawnictwa == ksiazkaVM.id_wydawnictwa);
            Ksiazki ksiazka = new Ksiazki
            {
                id_ksiazki=ksiazkaVM.id_ksiazki,
                data_wydania=ksiazkaVM.data_wydania,
                tytuł=ksiazkaVM.tytuł,
                id_wydawnictwa=ksiazkaVM.id_wydawnictwa
            };
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Ksiazki.Add(ksiazka);
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
        
    }
}