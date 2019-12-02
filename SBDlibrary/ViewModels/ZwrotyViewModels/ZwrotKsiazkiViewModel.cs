using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.ViewModels.ZwrotyViewModels
{
    public class ZwrotKsiazkiViewModel
    {
        public int id_zwrotu { get; set; }
        public int id_egzemplarza { get; set; }
        [Display(Name = "Tytuł")]
        public string tytul { get; set; }
        public int id_wypozyczenia { get; set; }
        [Display(Name = "Data zwrotu")]
        public DateTime data_zwrotu { get; set; }
        [Display(Name = "Kara")]
        public float kara { get; set; }

        public ZwrotKsiazkiViewModel(int id_zwrotu, int id_egzemplarza, string tytul,
            int id_wypozyczenia, DateTime data_zwrotu, float kara)
        {
            this.id_zwrotu = id_zwrotu;
            this.id_egzemplarza = id_egzemplarza;
            this.tytul = tytul;
            this.id_wypozyczenia = id_wypozyczenia;
            this.data_zwrotu = data_zwrotu;
            this.kara = kara;
        }
    }
}
