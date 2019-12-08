using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Autor
    {
        [Key]
        [Column(Order = 0)]
        public int id_autor { get; set; }
        [Required(ErrorMessage = "Pole 'Imię' jest wymagane.")]
        [StringLength(20, ErrorMessage = "Imię musi mieć co najmniej 2 oraz maksymalnie 20 znaków długości.", MinimumLength = 2)]
        [Column(Order = 1)]
        [Display(Name = "Imię")]
        public string imie { get; set; }
        [Required(ErrorMessage = "Pole 'Nazwisko' jest wymagane.")]
        [StringLength(20, ErrorMessage = "Nazwisko musi mieć co najmniej 2 oraz maksymalnie 20 znaków długości.", MinimumLength = 2)]
        [Column(Order = 2)]
        [Display(Name = "Nazwisko")]
        public string nazwisko { get; set; }
    }
}
