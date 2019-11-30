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
        public int id_zamowienia { get; set; }
        [Key]
        [Column(Order = 1)]
        public int id_ksiazki { get; set; }
        [Required(ErrorMessage = "Pole 'Ilość' jest wymagane.")]
        [Column(Order = 2)]
        [Display(Name = "Ilość")]
        [Range(1, 1000)]
        public int ilosc { get; set; }
        [ForeignKey("id_zamowienia")]
        public virtual Zamowienia Zamowienia { get; set; }
        [ForeignKey("id_ksiazki")]
        public virtual Ksiazki Ksiazki { get; set; }
    }
}
