using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Autorzy_Ksiazki
    {
        [Key]
        [Column(Order = 0)]
        public int id_autora { get; set; }
        [Key]
        [Column(Order = 1)]
        public int id_ksiazki { get; set; }
        [ForeignKey("id_autora")]
        public virtual Autor Autor { get; set; }
        [ForeignKey("id_ksiazki")]
        public virtual Ksiazki Ksiazki { get; set; }
    }
}
