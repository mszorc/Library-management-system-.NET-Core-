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

            KsiazkaViewModel ksiazkaVM = new KsiazkaViewModel();
                ksiazkaVM.id_ksiazki = ksiazka.id_ksiazki;
                ksiazkaVM.id_wydawnictwo = ksiazka.id_wydawnictwa;
                ksiazkaVM.tytuł = ksiazka.tytuł;
                ksiazkaVM.data_wydania = ksiazka.data_wydania;

            ksiazkaVM.Wydawnictwa = await _context.Wydawnictwa.FirstOrDefaultAsync(m => m.id_wydawnictwa == ksiazka.id_wydawnictwa);

            //kategorie
            var kategorie_ksiazki = await _context.Kategorie_Ksiazki.Where(m => m.id_ksiazki == ksiazka.id_ksiazki).ToListAsync();
            Kategorie kategoria = new Kategorie();
            List<string> listaKategorii = new List<string>();
            foreach(Kategorie_Ksiazki k in kategorie_ksiazki)
            {
                kategoria =await _context.Kategorie.FirstOrDefaultAsync(m => m.id_kategorii == k.id_kategorii);
                listaKategorii.Add(kategoria.nazwa);
            }
            ksiazkaVM.kategorieLista = listaKategorii;

            //autorzy
            var autorzy_ksiazki = await _context.Autorzy_Ksiazki.Where(m => m.id_ksiazki == ksiazka.id_ksiazki).ToListAsync();
            Autor autor = new Autor();
            List<string> listaAutorow = new List<string>();
            string autorRecord;
            foreach(Autorzy_Ksiazki a in autorzy_ksiazki)
            {
                autor = await _context.Autor.FirstOrDefaultAsync(m => m.id_autor == a.id_autora);
                autorRecord = autor.imie + " " + autor.nazwisko;
                listaAutorow.Add(autorRecord);
            }
            ksiazkaVM.autorzyLista = listaAutorow;

            return View(ksiazkaVM);

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
            //ViewData["kategorie"] = new MultiSelectList(_context.Kategorie, "nazwa", "nazwa");
            ViewData["wydawnictwa"] = new SelectList(_context.Wydawnictwa, "id_wydawnictwa", "nazwa");
            //List<Autor> autorzy = _context.Autor.Distinct().ToList();
            //List<string> autorzyImionaNazwiska = new List<string>();
            //foreach(Autor a in autorzy)
            //{
            //    autorzyImionaNazwiska.Add(a.imie + " " + a.nazwisko);
            //}
            //ViewData["autorzy"] = new SelectList(autorzyImionaNazwiska);
            
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bibliotekarz,Admin")]
        public async Task<ActionResult> Create([Bind("tytuł, data_wydania")]KsiazkaViewModel ksiazkaVM)
        {
            
            var wydawnictwo = await _context.Wydawnictwa.FirstOrDefaultAsync(m => m.id_wydawnictwa == ksiazkaVM.id_wydawnictwo);

            Ksiazki ksiazka = new Ksiazki();

                ksiazka.id_ksiazki = ksiazkaVM.id_ksiazki;
                ksiazka.id_wydawnictwa = wydawnictwo.id_wydawnictwa;
                ksiazka.tytuł = ksiazkaVM.tytuł;
                ksiazka.data_wydania = ksiazkaVM.data_wydania;

                ksiazka.Wydawnictwa = wydawnictwo;
            
            //kategorie_ksiazki
            Kategorie_Ksiazki kategorie_ksiazki = new Kategorie_Ksiazki();
            string[] splitKategorie = ksiazkaVM.kategorie.Split(new Char[] { ',' });
            Kategorie kategoria = new Kategorie();
            foreach(string k in splitKategorie)
            {
                kategoria = await _context.Kategorie.FirstOrDefaultAsync(m => m.nazwa == k);
                kategorie_ksiazki.id_kategorii = kategoria.id_kategorii;
                kategorie_ksiazki.id_ksiazki = ksiazkaVM.id_ksiazki;
                kategorie_ksiazki.Ksiazki = ksiazka;
                kategorie_ksiazki.Kategorie = kategoria;
                _context.Kategorie_Ksiazki.Add(kategorie_ksiazki);
            }

            //autorzy_ksiazki
            Autorzy_Ksiazki autorzy_ksiazki = new Autorzy_Ksiazki();
            string[] splitAutorzy = ksiazkaVM.autorzy.Split(new Char[] { ',' });
            Autor autor = new Autor();
            string[] splitWewn;
            foreach (string a in splitAutorzy)
            {
                splitWewn = a.Split(new Char[] { ' ' });
                autor = await _context.Autor.FirstOrDefaultAsync(m => m.nazwisko == splitWewn[1] && m.imie==splitWewn[0]);
                autorzy_ksiazki.id_autora = autor.id_autor;
                autorzy_ksiazki.id_ksiazki = ksiazka.id_ksiazki;
                autorzy_ksiazki.Autor = autor;
                autorzy_ksiazki.Ksiazki = ksiazka;
                _context.Autorzy_Ksiazki.Add(autorzy_ksiazki);
            }

            try
            {
                                
                if (ModelState.IsValid)
                {
                    _context.Ksiazki.Add(ksiazka);
                    await _context.SaveChangesAsync();
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