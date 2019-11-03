using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace SBDlibrary.Models
{
    public class Kategorie_Ksiazki
    {
        [Key]
        [Column(Order = 0)]
        public int id_kategorii { get; set; }
        [Key]
        [Column(Order = 1)]
        public int id_ksiazki { get; set; }
        [ForeignKey("id_kategorii")]
        public virtual Ksiazki Ksiazki { get; set; }
        [ForeignKey("id_ksiazki")]
        public virtual Kategorie Kategorie { get; set; }
    }
}
