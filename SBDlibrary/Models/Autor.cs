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
        [Required]
        [StringLength(20)]
        [Column(Order = 1)]
        public string imie { get; set; }
        [Required]
        [StringLength(20)]
        [Column(Order = 2)]
        public string nazwisko { get; set; }
    }
}
