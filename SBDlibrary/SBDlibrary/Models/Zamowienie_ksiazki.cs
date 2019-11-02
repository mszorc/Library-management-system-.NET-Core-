using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Zamowienie_ksiazki
    {
        [Required]
        public int ilosc { get; set; }
        [Key]
        public Zamowienia Zamowienia { get; set; }
        [Key]
        public virtual Ksiazki Ksiazki { get; set; }
    }
}
