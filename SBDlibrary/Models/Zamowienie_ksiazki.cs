using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Zamowienie_ksiazki
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }
        [Required]
        [Column(Order = 3)]
        public int ilosc { get; set; }
        [Column(Order = 1)]
        public virtual Zamowienia Zamowienia { get; set; }
        [Column(Order = 2)]
        public virtual Ksiazki Ksiazki { get; set; }
    }
}
