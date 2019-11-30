using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Dostawcy
    {
        [Key]
        [Column(Order = 0)]
        public int id_dostawcy { get; set; }

        [Required(ErrorMessage = "Pole 'Nazwa' jest wymagane.")]
        [StringLength(20, ErrorMessage = "Nazwa musi mieć co najmniej 2 oraz maksymalnie 20 znaków długości.", MinimumLength = 2)]
        [Display(Name = "Nazwa")]
        [Column(Order = 1)]
        public string nazwa { get; set; }

        [Required(ErrorMessage = "Pole 'Adres' jest wymagane.")]
        [StringLength(50, ErrorMessage = "Adres musi mieć co najmniej 2 oraz maksymalnie 50 znaków długości.", MinimumLength = 2)]
        [Display(Name = "Adres")]
        [Column(Order = 2)]
        public string adres { get; set; }
        public ICollection<Zamowienia> Zamowienia { get; set; }
    }
}
