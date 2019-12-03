using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Rezerwacje
    {
        public enum Status
        {
            odebrano, odwołano, aktualna
        }
        [Key]
        [Column(Order = 0)]
        public int id_rezerwacji { get; set; }

        [Required]
        [Column(Order = 1)]
        public int id_uzytkownika { get; set; }

        [Required]
        [Column(Order = 2)]
        public int id_egzemplarza { get; set; }

        [Required]
        [Column(Order = 3)]
        [Display(Name = "Data rezerwacji")]
        public DateTime data_rezerwacji { get; set; }
        [Required]
        [Column(Order = 4)]
        [Display(Name = "Data odbioru")]
        public DateTime data_odbioru { get; set; }
        [Required]
        [Column(Order = 5)]
        [Display(Name = "Status rezerwacji")]
        public Status? status_rezerwacji { get; set; }

        [ForeignKey("id_uzytkownika")]
        public virtual Uzytkownicy Uzytkownicy { get; set; }
   
        [ForeignKey("id_egzemplarza")]
        public virtual Egzemplarze Egzemplarze { get; set; }

    }
}
