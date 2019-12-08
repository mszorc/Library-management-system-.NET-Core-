using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.ViewModels.WypozyczeniaViewModels
{
    public class EgzemplarzViewModel
    {
        public int id_egzemplarza { get; set; }
        public int id_ksiazki { get; set; }
        public string tytul { get; set; }
        
        public EgzemplarzViewModel(int id_egzemplarza, int id_ksiazki, string tytul)
        {
            this.id_egzemplarza = id_egzemplarza;
            this.id_ksiazki = id_ksiazki;
            this.tytul = tytul;
        }

    }
}
