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
    public class SearchController : Controller
    {

        private readonly LibraryDbContext _context;

        public SearchController(LibraryDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Ksiazki.ToListAsync());
        }
      

        //[HttpPost]
        //public async Task<ActionResult> Search(SearchVM searchVM)
        //{
        //    List<Ksiazki> ksiazki = new List<Ksiazki>();

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        ksiazki = ksiazki.Where(k => k.tytuł.Contains(searchString)).ToList();
        //    }
        //    SearchVM list = new SearchVM();
        //    list.searchList = ksiazki;
        //    return View(ksiazki);
        //}
    }
}