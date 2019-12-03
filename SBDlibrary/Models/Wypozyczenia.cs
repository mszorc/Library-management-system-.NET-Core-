using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Wypozyczenia
    {
        [Key]
        [Column(Order = 0)]
        public int id_wypozyczenia { get; set; }

        [Required]
        [Column(Order = 1)]
        [Display(Name = "Użytkownik")]
        public int id_uzytkownika { get; set; }
        
        [Required]
        [Column(Order = 2)]
        [Display(Name = "Egzemplarz")]
        public int id_egzemplarza { get; set; }

        [Required]
        [Column(Order = 3)]
        [Display(Name = "Data wypożyczenia")]
        public DateTime data_wypozyczenia { get; set; }

        [Required]
        [Column(Order = 4)]
        [Display(Name = "Data zwrotu")]
        public DateTime data_zwrotu { get; set; }

        [ForeignKey("id_uzytkownika")]
        public virtual Uzytkownicy Uzytkownicy { get; set; }

        [ForeignKey("id_egzemplarza")]
        public virtual Egzemplarze Egzemplarze { get; set; }

       // public Zwroty Zwroty { get; set; }
    }
    public class SimpleModel
    {

        [Display(Name = "id_ksiazki")]
        public int id_ksiazki;
    }
}
