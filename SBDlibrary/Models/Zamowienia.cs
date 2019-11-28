using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Zamowienia
    {
        public enum Status
        {
            A, B, C
        }
        [Key]
        [Column(Order = 0)]
        public int id_zamowienia { get; set; }
        [Required]
        [Column(Order = 1)]
        public int id_dostawcy { get; set; }
        [Required]
        [Column(Order = 2)]
        public DateTime data_zamowienia { get; set; }
        [Required]
        [Column(Order = 3)]
        public Status? status_zamowienia { get; set; }
        [ForeignKey("id_dostawcy")]
        public virtual Dostawcy dostawcy { get; set; }
        public ICollection<Zamowienie_ksiazki> Zamowienie_ksiazki { get; set; }
    }
}
