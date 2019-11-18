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
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;
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
                ModelState.AddModelError(string.Empty, "book does not exist.");
                return View();
            }

            var ksiazka = _context.Ksiazki.FirstOrDefault(m => m.id_ksiazki == id);

            //if (ksiazka == null)
            //{
            //    ModelState.AddModelError(string.Empty, "Nie ma!.");
            //    return View();
            //}
            return View(ksiazka);

           // return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("tytuł, data_wydania, Wydawnictwa")]Ksiazki ksiazka)
        {
            ksiazka.Wydawnictwa = await _context.Wydawnictwa.FindAsync(1);
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
            return View(ksiazka);
        }
        public async Task<ActionResult> Create()
        {
          return View();
        }
        
    }
}