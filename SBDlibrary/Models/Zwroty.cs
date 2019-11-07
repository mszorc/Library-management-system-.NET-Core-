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
        [Column(Order = 2)]
        public DateTime data_zwrotu { get; set; }

        [Required]
        [Column(Order = 3)]
        public float kara { get; set; }

        [Column(Order = 1)]
        public virtual Wypozyczenia id_wypozyczenia { get; set; }

    }
}
