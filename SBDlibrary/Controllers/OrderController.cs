using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDlibrary.Models;

namespace SBDlibrary.Controllers
{
    public class OrderController : Controller
    {
        private readonly LibraryDbContext _context;

        public OrderController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var libraryDbContext = _context.Zamowienia.Include(z => z.dostawcy);
            return View(await libraryDbContext.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Zamowienie_ksiazki.Where(m => m.id_zamowienia == id).ToListAsync();
            var order = await _context.Zamowienia.FirstOrDefaultAsync(m => m.id_zamowienia == id);
            if (order == null)
            {
                return NotFound();
            }

            if (order.status_zamowienia == Zamowienia.Status.Zamówione)
            {
                ViewBag.Status = Zamowienia.Status.Zamówione;
            }
            
            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł");

            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("id_ksiazki,ilosc")] Zamowienie_ksiazki books)
        {
           /* if (id != books.id_zamowienia)
            {
                return NotFound();
            }*/

            if (ModelState.IsValid)
            {
                books.id_zamowienia = id;
                var duplicate = await _context.Zamowienie_ksiazki.FirstOrDefaultAsync(m => m.id_zamowienia == id && m.id_ksiazki == books.id_ksiazki);
                if (duplicate != null)
                {
                    duplicate.ilosc += books.ilosc;
                    _context.Update(duplicate);
                }
                else
                {
                    _context.Add(books);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Order", new { id = id});
            }

            ViewData["id_ksiazki"] = new SelectList(_context.Ksiazki, "id_ksiazki", "tytuł", books.id_ksiazki);
            return View(books);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            List<Zamowienia.Status> status = new List<Zamowienia.Status>();
            status.Add(Zamowienia.Status.Zamówione);
            ViewData["id_dostawcy"] = new SelectList(_context.Dostawcy, "id_dostawcy", "adres");
            ViewData["status"] = new SelectList(status);
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_zamowienia,id_dostawcy,data_zamowienia,status_zamowienia")] Zamowienia zamowienia)
        {
            if (ModelState.IsValid)
            {
                zamowienia.status_zamowienia = Zamowienia.Status.Zamówione;
                _context.Add(zamowienia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_dostawcy"] = new SelectList(_context.Dostawcy, "id_dostawcy", "adres", zamowienia.id_dostawcy);
            return View(zamowienia);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienia = await _context.Zamowienia.FindAsync(id);
            if (zamowienia == null)
            {
                return NotFound();
            }
            ViewData["id_dostawcy"] = new SelectList(_context.Dostawcy, "id_dostawcy", "adres", zamowienia.id_dostawcy);
            TempData["status"] = zamowienia.status_zamowienia;
            return View(zamowienia);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_zamowienia,id_dostawcy,data_zamowienia")] Zamowienia zamowienia)
        {
            if (id != zamowienia.id_zamowienia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                zamowienia.status_zamowienia = (Zamowienia.Status)TempData["status"];
                try
                {
                    _context.Update(zamowienia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZamowieniaExists(zamowienia.id_zamowienia))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_dostawcy"] = new SelectList(_context.Dostawcy, "id_dostawcy", "adres", zamowienia.id_dostawcy);
            return View(zamowienia);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienia = await _context.Zamowienia
                .Include(z => z.dostawcy)
                .FirstOrDefaultAsync(m => m.id_zamowienia == id);
            if (zamowienia == null)
            {
                return NotFound();
            }

            return View(zamowienia);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zamowienia = await _context.Zamowienia.FindAsync(id);
            _context.Zamowienia.Remove(zamowienia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CollectOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Zamowienia.FirstOrDefaultAsync(m => m.id_zamowienia == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.status_zamowienia == Zamowienia.Status.Odebrane)
            {
                return NotFound();
            }

            var books_list = await _context.Zamowienie_ksiazki.Where(m => m.id_zamowienia == order.id_zamowienia).ToListAsync();

            foreach(var book in books_list)
            {
                for (int i = 0; i < book.ilosc; i++)
                {
                    var copy = new Egzemplarze();
                    copy.id_ksiazki = book.id_ksiazki;
                    _context.Add(copy);
                }
            }

            order.status_zamowienia = Zamowienia.Status.Odebrane;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Order");
        }

        

        private bool ZamowieniaExists(int id)
        {
            return _context.Zamowienia.Any(e => e.id_zamowienia == id);
        }
    }
}
