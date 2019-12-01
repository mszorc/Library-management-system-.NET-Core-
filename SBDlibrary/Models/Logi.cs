using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Logi
    {
        [Key]
        [Column(Order = 0)]
        public int id_logu { get; set; }
        
       // [Required]
        [Column(Order = 1)]
        public int id_uzytkownika { get; set; }
        
        [Required]
        [StringLength(15)]
        [Column(Order = 2)]
        public string ip_urzadzenia { get; set; }
        
        [Required]
        [StringLength(50)]
        [Column(Order = 3)]
        public string komunikat { get; set; }

        [ForeignKey("id_uzytkownika")]
        public virtual Uzytkownicy Uzytkownicy { get; set; }

    }
}
