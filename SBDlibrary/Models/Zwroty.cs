using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Zwroty
    {
        [Key]
        [Column(Order = 0)]
        public int id_zwrotu { get; set; }
        [Required]
        [Column(Order = 1)]
        public int id_wypozyczenia { get; set; }

        [Required]
        [Column(Order = 2)]
        [Display(Name = "Data zwrotu")]
        public DateTime data_zwrotu { get; set; }

        [Required]
        [Column(Order = 3)]
        [Display(Name = "Kara")]
        public float kara { get; set; }

        [ForeignKey("id_wypozyczenia")]
        public virtual Wypozyczenia Wypozyczenia { get; set; }

    }
}
