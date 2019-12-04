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
        
        public string kategorie { get; set; }
        public List<string> kategorieLista { get; set; }
        public string autorzy { get; set; }
        public List<string> autorzyLista { get; set; }
        public int id_ksiazki { get; set; }
        public int id_wydawnictwo { get; set; }
        public string tytuł { get; set; }
        public DateTime data_wydania { get; set; }
        public virtual Wydawnictwa Wydawnictwa { get; set; }

    }
}
