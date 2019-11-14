using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Projekt2.ViewModels.EntryVM;
using Projekt2.BusinessLayer;
using Projekt2.Models;

namespace Projekt2.Controllers
{
    public class EntryController : Controller
    {
        // GET: Entry
        public ActionResult Index()
        {
            TempData.Keep("LoginName");
            EntryBL entryBL = new EntryBL();
            IndexVM vm = new IndexVM();
            vm.EntriesList = entryBL.List();
            return View(vm);
        }
        
        [Authorize]
        public ActionResult Add()
        {
            AddVM addvm = new AddVM();
            return View(addvm);
        }

        public ActionResult Notes(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Entry");
            TempData.Keep("LoginName");
            EntryBL entryBL = new EntryBL();
            List<Entry> entryList = entryBL.List();
            Entry entry = entryList.Where(u => u.ID == id).Single();
            NotesVM notes = new NotesVM();
            notes.entry = entry;
            return View("Notes", notes);
        }

        public ActionResult Search()
        {
            TempData.Keep("LoginName");
            SearchVM searchVM = new SearchVM();
            return View(searchVM);
        }

        
        [HttpPost]
        public ActionResult Search(SearchVM searchVM)
        {
            if (ModelState.IsValid)
            {
                TempData.Keep("LoginName");
                EntryBL entryBL = new EntryBL();
                SearchIndexVM sIndexVM = new SearchIndexVM();
                sIndexVM.EntriesList = entryBL.List().Where(u => u.Developer == searchVM.Developer).ToList();
                sIndexVM.EntriesList = sIndexVM.EntriesList.Where(u => u.Film == searchVM.Film).ToList();
                Session["searchList"] = sIndexVM.EntriesList;
                return RedirectToAction("SearchIndex", "Entry");
            }
            return View(searchVM);
        }
        
        public ActionResult SearchIndex()
        {
            TempData.Keep("LoginName");
            SearchIndexVM list = new SearchIndexVM();
            List<Entry> entrylist = (List<Entry>)Session["searchList"];
            list.EntriesList = entrylist;
            return View(list);
        }


        [Authorize]
        [HttpPost]
        public ActionResult Add(AddVM vm)
        {
            TempData.Keep("LoginName");
            if (ModelState.IsValid)
            {
                EntryBL entryBL = new EntryBL();
                Entry entry = new Entry();
                entry.Film = vm.Film;
                entry.Developer = vm.Developer;
                entry.Dillution = vm.Dillution;
                entry.Time = vm.Time;
                entry.ISO = vm.ISO;
                entry.Temperature = vm.Temperature;
                entry.Notes = vm.Notes;
                entry.User = (string)TempData["LoginName"];
                entryBL.AddEntry(entry);
                return RedirectToAction("Index", "Entry");
            }
            return View(vm);

        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            EntryBL entryBL = new EntryBL();
            List<Entry> entryList = entryBL.List();
            Entry entry = entryList.Where(u => u.ID == id).Single();
            entryBL.Delete(entry);
            return RedirectToAction("Index", "Entry");
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            TempData.Keep("LoginName");
            EntryBL entryBL = new EntryBL();
            List<Entry> entryList = entryBL.List();
            Entry entry = entryList.Where(u => u.ID == id).Single();
            EditVM editVM = new EditVM();
            editVM.Developer = entry.Developer;
            editVM.Dillution = entry.Dillution;
            editVM.Film = entry.Film;
            editVM.ISO = entry.ISO;
            editVM.Notes = entry.Notes;
            editVM.Temperature = entry.Temperature;
            editVM.Time = entry.Temperature;
            editVM.User = entry.User;
            editVM.User = (string)TempData["LoginName"];
            return View(editVM);
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult Edit(EditVM editVM)
        {
            TempData.Keep("LoginName");
            if (ModelState.IsValid)
            {
                EntryBL entryBL = new EntryBL();
                Entry entry = new Entry();
                entry.User = (string)TempData["LoginName"];
                entry.Time = editVM.Time;
                entry.Temperature = editVM.Temperature;
                entry.Notes = editVM.Notes;
                entry.ISO = editVM.ISO;
                entry.Film = editVM.Film;
                entry.Dillution = editVM.Dillution;
                entry.Developer = editVM.Developer;
                entry.ID = editVM.ID;
                entryBL.Edit(entry);
                return RedirectToAction("Index", "Entry");
            }
            return View(editVM);
        }
        
    }
}