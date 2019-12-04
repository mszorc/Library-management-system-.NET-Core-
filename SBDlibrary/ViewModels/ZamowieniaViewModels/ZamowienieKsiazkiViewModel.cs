using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.ViewModels.ZamowieniaViewModels
{
    public class ZamowienieKsiazkiViewModel
    {
        public int id_zamowienia { get; set; }
        public int id_ksiazki { get; set; }
        [Display(Name = "Tytuł")]
        public string tytul { get; set; }
        [Required(ErrorMessage = "Pole 'Ilość' jest wymagane.")]
        [Display(Name = "Ilość")]
        [Range(1, 1000)]
        public int ilosc { get; set; }

        public ZamowienieKsiazkiViewModel()
        {

        }

        public ZamowienieKsiazkiViewModel(int id_zamowienia, int id_ksiazki, string tytul, int ilosc)
        {
            this.id_zamowienia = id_zamowienia;
            this.id_ksiazki = id_ksiazki;
            this.tytul = tytul;
            this.ilosc = ilosc;
        }
    }
}
