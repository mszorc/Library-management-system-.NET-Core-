using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Uzytkownicy_role
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }
        [Column(Order = 2)]
        public virtual Role Role { get; set; }
        [Column(Order = 1)]
        public virtual Uzytkownicy Uzytkownicy { get; set; }
    }
}
