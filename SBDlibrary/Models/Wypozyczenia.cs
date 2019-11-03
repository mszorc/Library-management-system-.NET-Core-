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
        [Column(Order = 3)]
        public DateTime data_wypozyczenia { get; set; }

        [Required]
        [Column(Order = 4)]
        public DateTime data_zwrotu { get; set; }

        [Required]
        [Column(Order = 1)]
        [ForeignKey("id_uzytkownika")]
        public virtual Uzytkownicy id_uzytkownika { get; set; }

        [Required]
        [Column(Order = 2)]
        [ForeignKey("id_egzemplarza")]
        public virtual Egzemplarze id_egzemplarza { get; set; }

        public ICollection<Zwroty> Zwroty { get; set; }
    }
}
