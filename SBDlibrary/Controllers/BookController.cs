﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
            var number = new List<int>();
            var models = new List<WyswietlanieKsiazekModel>();

            var Ksiazki = from t in _context.Ksiazki
                     select t;
            foreach (Ksiazki k in Ksiazki)
            {
                
                WyswietlanieKsiazekModel pom = new WyswietlanieKsiazekModel();
                k.Wydawnictwa = await _context.Wydawnictwa.FirstOrDefaultAsync(m => m.id_wydawnictwa == k.id_wydawnictwa);
                pom.ksiazka = k;
               IQueryable<Egzemplarze> xd = from x in _context.Egzemplarze where (x.id_ksiazki == k.id_ksiazki) where (x.status == Egzemplarze.Status.Dostępny) select x;
                
                if (xd.Count()>0)
                    pom.dostepneEgzemplarze = "dostepne";
                else
                    pom.dostepneEgzemplarze = "niedostepne";
               models.Add(pom);
                xd = null;
            }

            // if (!HttpContext.User.IsInRole(""))
            // {

            //  }
           

           
            return View(models);
            //return View();
        }

        public async Task<ActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var ksiazka = await _context.Ksiazki.FirstOrDefaultAsync(m => m.id_ksiazki == id);
            ksiazka.Wydawnictwa = await _context.Wydawnictwa.FirstOrDefaultAsync(m => m.id_wydawnictwa == ksiazka.id_wydawnictwa);
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
        [Authorize(Roles = "Bibliotekarz,Admin")]
        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ksiazka = await _context.Ksiazki
               .FirstOrDefaultAsync(a => a.id_ksiazki == id);
            if (ksiazka == null)
            {
                return NotFound();
            }
            return View(ksiazka);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bibliotekarz,Admin")]
        public async Task<IActionResult> Edytuj(int id, Ksiazki ksiazka)
        {
            if (id != ksiazka.id_ksiazki)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ksiazka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KsiazkaExists(ksiazka.id_ksiazki))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ksiazka);
        }

        private bool KsiazkaExists(int id)
        {
            return _context.Ksiazki.Any(k => k.id_ksiazki == id);
        }

        [HttpGet]
        [Authorize(Roles = "Bibliotekarz,Admin")]
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
        [Authorize(Roles = "Bibliotekarz,Admin")]
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