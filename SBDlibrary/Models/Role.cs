using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Role
    {
        [Key]
        [Column(Order = 0)]
        public int id_roli { get; set; }
        [Required]
        [StringLength(12)]
        [Column(Order = 1)]
        public string nazwa { get; set; }
        public ICollection<Uzytkownicy_role> Uzytkownicy_role { get; set; }
    }
}
