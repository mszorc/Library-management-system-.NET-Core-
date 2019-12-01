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
        public string kategoria { get; set; }
        public string autor { get; set; }
        public string wydawnictwo { get; set; }
        public int id_ksiazki { get; set; }
        public string tytuł { get; set; }
        public DateTime data_wydania { get; set; }
        public virtual Wydawnictwa Wydawnictwa { get; set; }

    }
}
