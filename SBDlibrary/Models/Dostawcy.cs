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
        [StringLength(20)]
        [Required]
        [Column(Order = 1)]
        public string nazwa { get; set; }
        [StringLength(50)]
        [Required]
        [Column(Order = 2)]
        public string adres { get; set; }
        public ICollection<Zamowienia> Zamowienia { get; set; }
    }
}
