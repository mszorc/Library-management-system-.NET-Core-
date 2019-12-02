using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace SBDlibrary.Models
{
    public class Kategorie
    {
        [Key]
        [Column(Order = 0)]
        public int id_kategorii { get; set; }

        [Required(ErrorMessage = "Pole 'Nazwa' jest wymagane.")]
        [StringLength(20, ErrorMessage = "Nazwa musi mieć co najmniej 2 oraz maksymalnie 20 znaków długości.", MinimumLength = 2)]
        [Column(Order = 1)]
        [Display(Name = "Nazwa")]
        public string nazwa { get; set; }
    }
}
