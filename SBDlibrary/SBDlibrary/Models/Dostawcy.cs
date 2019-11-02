using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Dostawcy
    {
        [Key]
        public int id_dostawcy { get; set; }
        [StringLength(20)]
        [Required]
        public string nazwa { get; set; }
        [StringLength(50)]
        [Required]
        public string adres { get; set; }
        public ICollection<Zamowienia> Zamowienia { get; set; }
    }
}
