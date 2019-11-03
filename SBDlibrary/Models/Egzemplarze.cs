﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SBDlibrary.Models
{
    public class Egzemplarze
    {
        [Key]
        [Column(Order = 0)]
        public int id_egzemplarza { get; set; }
        [Required]
        [Column(Order = 1)]
        [ForeignKey("id_ksiazki")]
        public virtual Ksiazki Ksiazki { get; set; }
        public ICollection<Wypozyczenia> Wypozyczenia { get; set; }
        public ICollection<Rezerwacje> Rezerwacje { get; set; }
    }
}