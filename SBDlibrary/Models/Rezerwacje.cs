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
            A, B
        }
        [Key]
        [Column(Order = 0)]
        public int id_rezerwacji { get; set; }

        [Required]
        [Column(Order = 3)]
        public DateTime data_rezerwacji { get; set; }
        [Required]
        [Column(Order = 4)]
        public Status? status_rezerwacji { get; set; }

        [Required]
        [Column(Order = 1)]
        [ForeignKey("id_uzytkownika")]
        public virtual Uzytkownicy id_uzytkownika { get; set; }
        
        [Required]
        [Column(Order = 2)]
        [ForeignKey("id_egzemplarza")]
        public virtual Egzemplarze id_egzemplarza { get; set; }

        public ICollection<Uzytkownicy> Uzytkownicy { get; set; }
        public ICollection<Egzemplarze> Egzemplarze { get; set; }

    }
}
