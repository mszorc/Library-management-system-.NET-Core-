using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Wydawnictwa
    {
        [Key]
        [Column(Order = 0)]
        public int id_wydawnictwa { get; set; }
        [Required]
        [StringLength(20)]
        [Column(Order = 1)]
        public string nazwa { get; set; }
    }
}