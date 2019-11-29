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
        [Display(Name = "Id użytkownika")]
        public int id_uzytkownika { get; set; }
        [Required]
        [StringLength(50)]
        [Column(Order = 1)]
        [Display(Name = "Email")]
        public string email { get; set; }
        [Required]
        [StringLength(20)]
        [Column(Order = 2)]
        [Display(Name = "Hasło")]
        public string haslo { get; set; }
        [Required]
        [StringLength(20)]
        [Column(Order = 3)]
        [Display(Name = "Imię")]
        public string imie { get; set; }
        [Required]
        [StringLength(20)]
        [Column(Order = 4)]
        [Display(Name = "Nazwisko")]
        public string nazwisko { get; set; }
        [Required]
        [StringLength(50)]
        [Column(Order = 5)]
        [Display(Name = "Adres zamieszkania")]
        public string adres_zamieszkania { get; set; }
        public ICollection<Uzytkownicy_role> Uzytkownicy_role { get; set; }
        public ICollection<Logi> Logi { get; set; }
    }
}
