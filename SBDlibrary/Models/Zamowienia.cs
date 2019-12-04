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
            Zamówione, Odebrane
        }
        [Key]
        [Column(Order = 0)]
        public int id_zamowienia { get; set; }
        [Required(ErrorMessage = "Pole 'Dostawca' jest wymagane.")]
        [Column(Order = 1)]
        [Display(Name = "Dostawca")]
        public int id_dostawcy { get; set; }
        [Required(ErrorMessage = "Pole 'Data zamówienia' jest wymagane.")]
        [Column(Order = 2)]
        [Display(Name = "Data zamówienia")]
        public DateTime data_zamowienia { get; set; }
        [Required(ErrorMessage = "Pole 'Status zamówienia' jest wymagane.")]
        [Column(Order = 3)]
        [Display(Name = "Status zamówienia")]
        public Status? status_zamowienia { get; set; }
        [ForeignKey("id_dostawcy")]
        public virtual Dostawcy dostawcy { get; set; }
        public ICollection<Zamowienie_ksiazki> Zamowienie_ksiazki { get; set; }
    }
}
