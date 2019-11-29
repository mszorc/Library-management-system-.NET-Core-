using Microsoft.AspNetCore.Mvc.Rendering;
using SBDlibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.ViewModels.BookViewModels
{
    public class KsiazkaViewModel
    {
        public List<Kategorie> kategorieListVM = new List<Kategorie>();
        public SelectList kategorie { get; set; }
        public int id_ksiazki { get; set; }
        public int id_wydawnictwa { get; set; }
        public string tytuł { get; set; }
        public DateTime data_wydania { get; set; }
        public virtual Wydawnictwa Wydawnictwa { get; set; }

    }
}
