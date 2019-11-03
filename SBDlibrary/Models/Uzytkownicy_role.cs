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
        public int id_roli { get; set; }
        [Key]
        [Column(Order = 1)]
        public int id_uzytkownika { get; set; }
        [ForeignKey("id_roli")]
        public virtual Role Role { get; set; }
        [ForeignKey("id_uzytkownika")]
        public virtual Uzytkownicy Uzytkownicy { get; set; }
    }
}
