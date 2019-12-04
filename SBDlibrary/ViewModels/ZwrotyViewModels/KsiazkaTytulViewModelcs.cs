using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.ViewModels.ZwrotyViewModels
{
    public class KsiazkaTytulViewModel
    {
        [Display(Name = "Tytuł")]
        public int id_wypozyczenia { get; set; }
        public int id_ksiazki { get; set; }
        public string tytul { get; set; }

        public KsiazkaTytulViewModel()
        {

        }

        public KsiazkaTytulViewModel(int id_wypozyczenia, int id_ksiazki, string tytul)
        {
            this.id_wypozyczenia = id_wypozyczenia;
            this.id_ksiazki = id_ksiazki;
            this.tytul = tytul;
        }
    }
}
