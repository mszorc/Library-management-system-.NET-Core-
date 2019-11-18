using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Uzytkownicy
    {
        [Key]
        [Column(Order = 0)]
        public int id_uzytkownika { get; set; }
        [Required]
        [StringLength(50)]
        [Column(Order = 1)]
        public string email { get; set; }
        [Required]
        [StringLength(20)]
        [Column(Order = 2)]
        public string haslo { get; set; }
        [Required]
        [StringLength(20)]
        [Column(Order = 3)]
        public string imie { get; set; }
        [Required]
        [StringLength(20)]
        [Column(Order = 4)]
        public string nazwisko { get; set; }
        [Required]
        [StringLength(50)]
        [Column(Order = 5)]
        public string adres_zamieszkania { get; set; }
        public ICollection<Uzytkownicy_role> Uzytkownicy_role { get; set; }
        public ICollection<Logi> Logi { get; set; }
    }
}
