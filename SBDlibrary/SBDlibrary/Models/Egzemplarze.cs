using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Egzemplarze
    {
        [Key]
        public int id_egzemplarza { get; set; }
        [Required]
        public virtual Ksiazki Ksiazki { get; set; }
        public ICollection<Wypozyczenia> Wypozyczenia { get; set; }
        public ICollection<Rezerwacje> Rezerwacje { get; set; }
    }
}
