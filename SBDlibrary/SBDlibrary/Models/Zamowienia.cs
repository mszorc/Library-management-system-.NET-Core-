using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int id_zamowienia { get; set; }
        [Required]
        public int id_dostawcy { get; set; }
        [Required]
        public DateTime data_zamowienia { get; set; }
        [Required]
        public Status? status_zamowienia { get; set; }
        public virtual Dostawcy dostawcy { get; set; }
        public ICollection<Zamowienie_ksiazki> Zamowienie_ksiazki { get; set; }
    }
}
